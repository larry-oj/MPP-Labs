string text = File.ReadAllText("./text.txt");
string[] wordList = new string[Int16.MaxValue];
int wordListLength = 0;

// dictionary
int dictRes = -1;
string[] dictKey = new string[Int16.MaxValue];
int[][] dictValue = new int[Int16.MaxValue][];
int[] dictCount = new int[Int16.MaxValue];
int[] dictValueLength = new int[Int16.MaxValue];
int dictLength = 0;

int i = 0;          // iterator 1
int j = 0;          // iterator 2
int n = 0;          // iterator 3
int start = 0;      // start index
int end = 0;        // stop index

bool jumpToNextLine = false;
int currentLine = 1;
int lastI = -1;

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

            if ((@char < 'A' || (@char > 'Z' && @char < 'a') || @char > 'z') && @char != ' ')
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

        lastI = i;

        // reset
        i = 0;
        j = 0;
        start = 0;
        end = 0;

        splitLine:
            if (line.Length == 0) goto splitLineEnd;
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

                if (word != " " && word != "for" && word != "the" && word != "in" && word != "a") 
                {
                    wordList[wordListLength] = word;
                    wordListLength++;
                }

                start = end + 2;
            }

            i++;
            
            if (i < line.Length && line.Length != 0)
            {
                goto splitLine;
            }
        splitLineEnd:
        i = lastI;
    }

    i++;
    if (jumpToNextLine)
    {
        lastI = i;

        i = 0;
        j = 0;

        if (wordListLength == 0) goto jumpNextLine;

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

            

            
            checkDictEnd:
            j = 0;
                int cPage = currentLine;
                int pageNum = 1;
                calcCurrentPage:
                    if(cPage - 45 > 0) {
                        pageNum++;
                    }

                    cPage -= 45;
                    if(cPage > 0) {
                        goto calcCurrentPage;
                    }

                
                if(dictRes != -1) {
                    findWord:
                        if (dictValue[dictRes][j] == pageNum) {
                            goto findWordEnd;
                        }
                        else if (dictValue[dictRes][j] == 0) {
                            dictValue[dictRes][j] = pageNum;
                            dictValueLength[dictRes]++;
                            goto findWordEnd;
                        }

                        j++;
                        if (j < 100) {
                            goto findWord;
                        }
                    findWordEnd:
                    j = 0;
                    dictCount[dictRes]++;
                }    
                else {
                    dictKey[dictLength] = wordList[i];
                    dictValue[dictLength] = new int[100];
                    dictValue[dictLength][0] = pageNum;
                    dictCount[dictLength]++;
                    dictValueLength[dictLength]++;
                    dictLength++;
                }
                if(i != wordListLength - 1) {
                    i++;
                    goto dictLoop;
                }

        jumpNextLine:
        i = lastI;
        i++;
        jumpToNextLine = false;
        start = i;
        currentLine++;
    }
    if (i < text.Length)
    {
        wordListLength = 0;
        wordList = new string[Int16.MaxValue];
        goto splitToWords;
    }



i = 0;
j = 0;

string tmpWord;
int[] tmpPages;
int tmpCount;
int tmpLength;

outer:
    if(i >= dictLength - 1) {
        goto endouter;
    }
    j = 0;
startinner:
    if (j >= dictLength - 1) {
        goto endinner;
    }
    n = 0;
sort:
    if (dictKey[j][n] == dictKey[j + 1][n]) {
        if (n < dictKey[j].Length - 1 && n < dictKey[j + 1].Length - 1) {
            n++;
            goto sort;
        }
    }
    else if (dictKey[j][n] < dictKey[j + 1][n]) {
        goto noswap;
    }

    tmpWord = dictKey[j];
    tmpPages = dictValue[j];
    tmpCount = dictCount[j];
    tmpLength = dictValueLength[j];
    dictKey[j] = dictKey[j + 1];
    dictValue[j] = dictValue[j + 1];
    dictCount[j] = dictCount[j + 1];
    dictValueLength[j] = dictValueLength[j + 1];
    dictKey[j + 1] = tmpWord;
    dictValue[j + 1] = tmpPages;
    dictCount[j + 1] = tmpCount;
    dictValueLength[j + 1] = tmpLength;
noswap:
    j++;
    goto startinner;
endinner:
    i++;
    goto outer;
endouter:

i = 0;
write:
    if (dictCount[i] < 100) {
        Console.Write(dictKey[i] + " - ");

        j = 0;
        writePages:
            Console.Write(dictValue[i][j]);

            if (j < dictValueLength[i] - 1) {
                j++;
                Console.Write(", "); // добавить запятую, если еще есть станицы
                goto writePages;
            }
        Console.Write('\n');
    }
    
    if (i != dictLength - 1) {
        i++;
        goto write;
    }