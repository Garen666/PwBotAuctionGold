using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2 {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var  forms = new Form1();

            // указываем поля для селекта
            forms.comboBox1.DisplayMember = "Text";
            forms.comboBox1.ValueMember = "Value";

            Application.Run(forms);

            
        }
    }
}
