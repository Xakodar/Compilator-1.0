


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
