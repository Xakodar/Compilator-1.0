using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

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
                // Если выбрано "Нет", продолжаем без сохранения изменений
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
                    System.IO.File.WriteAllText(currentFile, "");
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


        /*СПРАВКА*/


        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string helpFilePath = Path.Combine(Application.StartupPath, "help.html");

                // Запускаем браузер по умолчанию с этим файлом
                Process.Start(helpFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось открыть файл справки: " + ex.Message);
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Пока что лабораторная работа №1, но потом будет РГЗ крутое",
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
            // Получаем исходный код из текстового поля
            string sourceCode = richTextBox1.Text;

            // Создаем объект сканера и выполняем анализ
            Scanner scanner = new Scanner();
            var tokens = scanner.Scan(sourceCode);

            // Очищаем DataGridView
            dataGridViewoutput.Visible = false;
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
            // Вывод ошибок, обнаруженных парсером, в текстовое поле
            if (parser.Errors.Any())
            {
                textBoxErrors.Text = string.Join(Environment.NewLine, parser.Errors.Select(err => err.ToString()));
            }
            else
            {
                textBoxErrors.Text = "Ошибок не обнаружено";
            }
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
    }
}
