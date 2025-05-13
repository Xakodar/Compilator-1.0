


Суть 5-ой лабораторной -- Выполнить разбор строки в виде тетрад

1) Реализовать в текстовом редакторе поиск лексических и синтаксических ошибок для грамматики G[<E>]. Реализовать данную КС-граммматику методом рекурсивного спуска:

1. E → TA 

2. A → ε | + TA | - TA 

3. T → ОВ 

4. В → ε | *ОВ | /ОВ 

5. О → id | (E) 

6. id → letter {letter}

2) Реализовать алгоритм записи выражений в форме тетрад.



Примеры верных строк:

1. a + b * (c - d)
2. ((a+b)/(c*d)) - k - t
3. c = a + b

Тестовые примеры:

![изображение](https://github.com/user-attachments/assets/408664b2-d03d-441b-82f0-c93239c2af34)

![изображение](https://github.com/user-attachments/assets/bd0608aa-3204-408b-9ba3-bab4f3be16d0)

![изображение](https://github.com/user-attachments/assets/5afe7477-9123-431a-8e75-afbe11ac7daf)

6 лабораторная:

Решение 1 блока задач:


private static readonly Regex PassportRx = new Regex(@"\b\d{4}[-\s]?\d{6}\b");

Решение 2 блока задач:


private static readonly Regex CommentRx = new Regex(@"(?s)(\"""".*?\""""|'''.*?''')", RegexOptions.Singleline);

Решение 3 блока задач:

private static readonly Regex HslRx = new Regex(@"\bhsl\(\s*(?:3[0-5]\d|360|[12]?\d{1,2})\s*,\s*(?:100|[1-9]?\d)%\s*,\s*(?:100|[1-9]?\d)%\s*\)");


Тестовые примеры:

![изображение](https://github.com/user-attachments/assets/91259fe3-de65-4d0b-a8b1-4bdacc8503fa)


![изображение](https://github.com/user-attachments/assets/ad6c4913-1c5f-42b4-bc3d-07b694d9c961)


![изображение](https://github.com/user-attachments/assets/e98a5ab4-cb56-444a-8804-3b88b6617e68)

