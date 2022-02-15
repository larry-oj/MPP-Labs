string text = File.ReadAllText("./text.txt");
string[] wordList = new string[Int16.MaxValue];
int wordListLength = 0;

// dictionary
int dictRes = -1;
string[] dictKey = new string[Int16.MaxValue];
int[] dictValue = new int[Int16.MaxValue];
int dictLength = 0;

int i = 0;          // iterator 1
int j = 0;          // iterator 2
int start = 0;      // start index
int end = 0;        // stop index

bool jumpToNextLine = false;

splitToWords:
    char @char = text[i];

    if (@char == '\r' || i == text.Length - 1)
    {
        if (i == text.Length - 1)
        {
            end = i;
        }
        else
        {
            end = i - 1;
        }

        jumpToNextLine = true;
        
        string line = "";

        j = start;
        saveLine:
            @char = text[j];

            // ignore punctuation marks 
            if (@char == ',' || @char == '.' || @char == '!' || @char == '?' || @char == '-')
            {
                goto saveLineEnd;
            }
            
            line += @char;
            
            saveLineEnd:
                j++;
                if (j <= end) 
                {
                    goto saveLine;
                }

        int lastI = i;

        // reset
        i = 0;
        j = 0;
        start = 0;
        end = 0;

        splitLine:
            @char = line[i];

            if (@char == ' ' || i == line.Length - 1)
            {
                string word = "";

                if (i == line.Length - 1)
                {
                    end = i;
                }
                else
                {
                    end = i - 1;
                }

                j = start;
                saveWord:
                    @char = line[j];

                    if (@char >= 'A' && @char < 'a')
                    {
                        @char = (char)(@char + 32);
                    }

                    word += @char;

                    j++;
                    if (j <= end) 
                    {
                        goto saveWord;
                    }

                if (word != "" && word != "for" && word != "the" && word != "in") 
                {
                    wordList[wordListLength] = word;
                    wordListLength++;
                }

                start = end + 2;
            }

            i++;
            
            if (i < line.Length)
            {
                goto splitLine;
            }

        i = lastI;
    }

    i++;
    if (jumpToNextLine)
    {
        i++;
        jumpToNextLine = false;
        start = i;
    }
    if (i < text.Length)
    {
        goto splitToWords;
    }

// reset
i = 0;
j = 0;

dictLoop:
    dictRes = -1;

    checkDict:
        if(dictKey[j] == wordList[i]) {
            dictRes = j;
            goto checkDictEnd;
        }
        if(j != dictLength) {
            j++;
            goto checkDict;
        }

    j = 0;
    checkDictEnd:
        if(dictRes != -1) {
            dictValue[dictRes]++;
        }    
        else {
            dictKey[dictLength] = wordList[i];
            dictValue[dictLength] = 1;
            dictLength++;
        }
        if(i != wordListLength - 1) {
            i++;
            goto dictLoop;
        }

// reset again
i = 0;
j = 0;

string tmpStr = "";
int tmpInt = 0;

outer:
    if(i >= dictLength - 1) {
        goto endouter;
    }
    j = 0;
startinner:
    if (j >= dictLength - 1) {
        goto endinner;
    }
    if (dictValue[j] > dictValue[j + 1]) {
        goto noswap;
    }
    tmpStr = dictKey[j];
    tmpInt = dictValue[j];
    dictKey[j] = dictKey[j + 1];
    dictValue[j] = dictValue[j + 1];
    dictKey[j + 1] = tmpStr;
    dictValue[j + 1] = tmpInt;
noswap:
    j++;
    goto startinner;
endinner:
    i++;
    goto outer;
endouter:

// reset once again
i = 0;

write:
    Console.WriteLine(dictKey[i] + " - " + dictValue[i]);
    if (i != dictLength - 1) {
        i++;
        goto write;
    }