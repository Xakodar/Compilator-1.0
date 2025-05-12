using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq; 
using System.Net.NetworkInformation;
using System.Windows.Forms;
using TetradApp;

namespace Laba1
{
    public partial class Compiler : Form
    {
        private string currentFile = string.Empty;

        public Compiler()
        {
            InitializeComponent();
        }

        /*ФАЙЛ*/


        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Если в редакторе есть текст (возможно, изменения)
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения в текущем документе?",
                    "Сохранение изменений",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Вызываем функцию "Сохранить как" для сохранения изменений
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    // Если пользователь отменил, прерываем создание нового файла
                    return;
                }
            }

            // Открываем диалоговое окно для создания нового файла
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Создать новый документ",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currentFile = sfd.FileName;
                try
                {
                    // Физически создаём пустой файл
                    File.WriteAllText(currentFile, "");
                    // Очищаем редактор для нового документа
                    richTextBox1.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при создании файла: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                currentFile = ofd.FileName;
                richTextBox1.Text = File.ReadAllText(currentFile);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                // Если файл ещё не был сохранён - вызываем "Сохранить как"
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
            else
            {
                // Сохраняем в текущий файл
                File.WriteAllText(currentFile, richTextBox1.Text);
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                currentFile = sfd.FileName;
                File.WriteAllText(currentFile, richTextBox1.Text);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /*ПРАВКА*/


        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanUndo)
            {
                richTextBox1.Undo();
            }
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.CanRedo)
            {
                richTextBox1.Redo();
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.Cut();
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionLength > 0)
            {
                richTextBox1.SelectedText = string.Empty;
            }
        }

        private void выделитьВсёToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        /*ТЕКСТ*/

        private void постановкаЗадачиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                    "В результате выполнения курсовой работы был разработан синтаксический анализатор (парсер) для объявления списка с инициализацией на языке Python. Целью работы является создание программы, способной правильно анализировать и интерпретировать синтаксис списков с инициализацией в Python, используя грамматику и алгоритм для синтаксического анализа.\n\n" +
                    "Задача парсера – корректно обработать конструкции, такие как:\n" +
                    "- объявление списка с числами: [1, 2, 3];\n" +
                    "- строковые списки: [\"a\", \"b\", \"c\"];\n" +
                    "- смешанные типы данных: [1, \"apple\", 3.14, True].\n\n" +
                    "В рамках работы также был реализован механизм диагностики синтаксических ошибок и их нейтрализации.",
                    "Постановка задачи",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void грамматикаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("1.\t‹List› → ‹Letter›‹I›\r\n2.\t‹I› → ‹Letter›‹I›\r\n3.\t‹I› → ‹=›‹O›\r\n4.\t‹O› → ‹[›‹P›\r\n5.\t‹P› → ‹+|–›‹T›\r\n6.\t‹P› → ‹Digit›‹TR›\r\n7.\t‹P› → ‹”›‹S›\r\n8.\t‹P› → ‹]›‹E›\r\n9.\t‹T› → ‹Digit›‹TR›\r\n10.\t‹TR› → ‹Digit›‹TR›\r\n11.\t‹TR› → ‹,›‹P›\r\n12.\t‹TR› → ‹.›‹F›\r\n13.\t‹TR› → ‹]›‹E›\r\n14.\t‹F› → ‹Digit›‹FR›\r\n15.\t‹FR› → ‹Digit›‹FR›\r\n16.\t‹FR› → ‹,›‹P›\r\n17.\t‹FR› → ‹]›‹E›\r\n18.\t‹S› → ‹Letter | Digit›‹SR›\r\n19.\t‹SR› → ‹Letter | Digit›‹SR›\r\n20.\t‹SR› → ‹”›‹A›\r\n21.\t‹A› → ‹,›‹P›\r\n22.\t‹A› → ‹]›‹E›\r\n23.\t‹E› → ‹;›\r\n•\t‹Digit› → “0” | “1” | “2” | “3” | “4” | “5” | “6” | “7” | “8” | “9”\r\n•\t‹Letter› → “a” | “b” | “c” | ... | “z” | “A” | “B” | “C” | ... | “Z”\r\n", 
                "Грамматика", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void классификацияГрамматикиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Согласно классификации Хомского, грамматика G[‹List›] является автоматной.\r\nПраворекурсивные правила:\r\nПравила, где рекурсивный вызов нетерминала находится в крайней правой позиции, что соответствует форме A → aB.\r\nВ данной грамматике такими являются:\r\n(2) ‹I› → ‹Letter›‹I›\r\n(10) ‹TR› → ‹Digit›‹TR›\r\n(15) ‹FR› → ‹Digit›‹FR›\r\n(19) ‹SR› → ‹Letter | Digit›‹SR›\r\nОстальные правила не содержат рекурсии или завершают цепочку продукций терминальным символом (правило (23) ‹E› → ‹;›).\r\nПоскольку все правила продукции имеют форму либо A → aB, либо A → a, грамматика является праворекурсивной и, следовательно, соответствует автоматной грамматике (регулярной грамматике, тип-3 по классификации Хомского). Это удовлетворяет требованию о том, что все правила должны быть либо леворекурсивными, либо праворекурсивными – в нашем случае они однородно праворекурсивные.\r\n", 
                "Классификация грамматики", 
                MessageBoxButtons.OK, 
                MessageBoxIcon.Information);
        }

        private void методАнализаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                    "Для синтаксического анализа использован метод, основанный на автоматной грамматике, что позволяет эффективно обрабатывать конструкции объявлений списков. Программа использует праворекурсивные правила грамматики, что делает её достаточно простой и быстрой в выполнении.\n\n" +
                    "Синтаксический анализатор использует стек и работает по принципу последовательного чтения входной цепочки символов. Если на текущем шаге встречается ошибка, то применяется метод нейтрализации ошибок, при котором некорректный символ или конструкция удаляется из цепочки до тех пор, пока не будет найдено допустимое продолжение.\n\n" +
                    "Также для повышения устойчивости к ошибкам в процессе разбора были реализованы механизмы для пропуска некорректных данных, что помогает избежать неожиданных сбоев.",
                    "Метод анализа",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void диагностикаИНейтрализацияОшибокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                    "Для диагностики синтаксических ошибок используется алгоритм, который анализирует входную строку и ищет места, где структура данных не соответствует ожидаемой грамматике. В случае нахождения ошибки, программа автоматически применяет метод нейтрализации ошибок.\n\n" +
                    "Метод нейтрализации заключается в удалении символа из текущей цепочки и попытке продолжить анализ с этого момента. Это позволяет корректно обработать ошибочные данные и продолжить выполнение программы. Такой подход позволяет избежать остановки выполнения программы при возникновении ошибок, а также помогает обработать некорректные, но исправляемые данные.",
                    "Диагностика и нейтрализация ошибок",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void текстовыйПримерToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                    "Пример 1:\nСписок с числами:\n\nlist_example = [1, 2, 3]\n\nРезультат: Синтаксический анализ успешен, программа интерпретирует список как коллекцию целых чисел.\n\n" +
                    "Пример 2:\nСписок с разными типами данных:\n\nlist_example = [1, \"apple\", 3.14, True]\n\n",
                    "Тестовый пример",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
        }

        private void списокЛитературыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "1. Шорников Ю.В. Теория и практика языковых процессоров : учеб. пособие / Ю.В. Шорников. – Новосибирск: Изд-во НГТУ, 2022.\n\n" +
                "2. Gries D. Designing Compilers for Digital Computers. New York, Jhon Wiley, 1971. 493 p.\n\n" +
                "3. Теория формальных языков и компиляторов [Электронный ресурс] / Электрон. дан. URL: https://dispace.edu.nstu.ru/didesk/course/show/8594, свободный. Яз.рус. (дата обращения 25.03.2025).\n\n",
                "Список литературы",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void исходныйКодПрограммыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SourceCodeForm sourceCodeForm = new SourceCodeForm();
            sourceCodeForm.ShowDialog();
        }


        /*СПРАВКА*/


        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "help.html");

            try
            {
                // Сохраняем встроенный HTML в файл
                File.WriteAllText(tempPath, Properties.Resources.help);
                // Открываем в браузере
                Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл \"Грамматика\": " + ex.Message);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Данная программа представляет собой компилятор, в частности лексер, который сканирует строку и разбивает её на лексемы и в случае нахождения ошибки заканчивает сканирование",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        /*ПУСК*/


        private void toolStripDropDownButton4_Click(object sender, EventArgs e)
        {
            toolStripButtonRun_Click(sender, e);
        }

        /*ПАНЕЛЬ ИНСТРУМЕНТОВ*/


        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            создатьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            открытьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            сохранитьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonUndo_Click(object sender, EventArgs e)
        {
            отменитьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonRedo_Click(object sender, EventArgs e)
        {
            повторитьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            копироватьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            вырезатьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            вставитьToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonRun_Click(object sender, EventArgs e)
        {
            /*// Получаем исходный код из текстового поля
            string sourceCode = richTextBox1.Text;

            // Создаем объект сканера и выполняем анализ
            Scanner scanner = new Scanner();
            var tokens = scanner.Scan(sourceCode);

            // Очищаем DataGridView
            textBoxErrors.Visible = false;
            dataGridViewoutput.Visible = true;
            dataGridViewoutput.Rows.Clear();
            dataGridViewoutput.Columns.Clear();

            // Добавляем столбцы
            dataGridViewoutput.Columns.Add("colCode", "Код");
            dataGridViewoutput.Columns.Add("colType", "Тип лексемы");
            dataGridViewoutput.Columns.Add("colLexeme", "Лексема");
            dataGridViewoutput  .Columns.Add("colLine", "Строка");
            dataGridViewoutput.Columns.Add("colStart", "Начальная позиция");
            dataGridViewoutput.Columns.Add("colEnd", "Конечная позиция");

            // Заполняем DataGridView данными токенов
            foreach (var token in tokens)
            {
                dataGridViewoutput.Rows.Add(
                    (int)token.Code,
                    token.Type,
                    token.Lexeme,
                    token.Line,
                    token.StartPos,
                    token.EndPos
                );
            }

            ListParser parser = new ListParser();
            parser.Parse(sourceCode);
            textBoxErrors.Visible = true;
            dataGridViewoutput.Visible = false;
            // Вывод ошибок, обнаруженных парсером, в текстовое поле
            if (parser.Errors.Any())
            {
                textBoxErrors.Text = string.Join(Environment.NewLine, parser.Errors.Select(err => err.ToString()));
            }
            else
            {
                textBoxErrors.Text = "Ошибок не обнаружено";
            }*/
            // сброс подсветки
            richTextBox1.SelectAll();
            richTextBox1.SelectionBackColor = Color.White;
            textBoxErrors.Clear();

            var lexer = new Lexer(richTextBox1.Text);
            lexer.Tokenize();
            foreach (var le in lexer.Errors)
            {
                richTextBox1.Select(le.Position, le.Length);
                richTextBox1.SelectionBackColor = Color.Yellow;
                textBoxErrors.AppendText($"Лексическая ошибка: {le.Message} (позиц. {le.Position})\r\n");
            }

            if (lexer.Errors.Any())
            {
                textBoxErrors.Focus();
                return;
            }

            var parser = new Parser(lexer.Tokens);
            parser.Parse();
            foreach (var se in parser.Errors)
            {
                richTextBox1.Select(se.Position, se.Length);
                richTextBox1.SelectionBackColor = Color.LightPink;
                textBoxErrors.AppendText($"Синтаксическая ошибка: {se.Message} (позиц. {se.Position})\r\n");
            }

            if (parser.Errors.Any())
            {
                textBoxErrors.Focus();
                dataGridViewoutput.Rows.Clear();  // не выводим тетрады
                return;
            }

            // если ошибок нет — выводим тетрады
            dataGridViewoutput.Columns.Clear();
            dataGridViewoutput.Columns.Add("op", "op");
            dataGridViewoutput.Columns.Add("arg1", "arg1");
            dataGridViewoutput.Columns.Add("arg2", "arg2");
            dataGridViewoutput.Columns.Add("result", "result");
            dataGridViewoutput.Rows.Clear();

            foreach (var q in parser.Quads)
                dataGridViewoutput.Rows.Add(q.Op, q.Arg1, q.Arg2, q.Result);
        }

        private void toolStripButtonHelp_Click(object sender, EventArgs e)
        {
            вызовСправкиToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButtonAbout_Click(object sender, EventArgs e)
        {
            оПрограммеToolStripMenuItem_Click(sender, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Если в редакторе есть текст (возможно, изменения)
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Сохранить изменения в текущем документе?",
                    "Сохранение изменений",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Вызываем функцию "Сохранить как" для сохранения изменений
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
            }
        }

        private void buttonTethrads_Click(object sender, EventArgs e)
        {
            textBoxErrors.Visible = false;
            dataGridViewoutput.Visible = true;
        }

        private void buttonErrors_Click(object sender, EventArgs e)
        {
            dataGridViewoutput.Visible = false;
            textBoxErrors.Visible = true;
        }
    }
}
