using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace compiler
{
    public enum token { lparn_sy,  id, rparn_sy, num_sy, enter_sy,print_sy,equal_sy,if_sy,then_sy,greater_sy, greater_equal_sy, less_sy, less_equal_sy,plus_sy, mult_sy, div_sy, minus_sy,from_sy,to_sy,step_sy , error_sy, end_source_sy,int_sy,bool_sy,string_sy }
    
  
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if(textBox1.Text=="احمد")
            parser p = new parser(textBox1.Text);
            p.sample_parser();
       
            listBox1.Items.Add( p.local);
            MessageBox.Show(p.local);
        }
    }
    public class parser
    {
        public int index;
        public int pos = 0;
        public string[] line;
        public string local;
        public token curr_token;


        public parser(string fileName)
        {

            line = File.ReadAllLines(fileName);
          //  line[line.Length - 1] += " ";
            index = 0;
            local = "";
        }
        token get_token()
        {
            char ch;
            string s = "";
            int state = 0;
            for (int j = pos; j <= line.Length - 1; j++)
            {
                
                for (int i = index; i <= line[j].Length-1; i++)
                {
                    switch (state)
                    {
                        case 0:
                            ch = line[j][i];
                            if (isspace(ch)) { s = ch.ToString(); state = 0; }
                            else if (isdigit(ch)) { s = ch.ToString(); state = 1; }


                            else if (isalpha(ch)) { s = ch.ToString(); state = 2; }
                            else if (ch == '-') state = 3;
                            else if (ch == '=') state = 4;
                            else if (ch == '<') state = 5;
                            else if (ch == '>') state = 6;
                            else if (ch == '+') state = 7;
                            else if (ch == '*') state = 8;
                            else if (ch == '(') state = 9;
                            else if (ch == ')') state = 10;
                            else if (ch == '/') state = 11;

                            else state = 11;
                            break;

                        case 1:
                            ch = line[j][i];  //5555
                            if (isdigit(ch))
                            {
                                s += ch;
                                state = 1;
                            }
                            else
                            {
                                //i--;
                                index = i;
                                return token.num_sy;
                            }
                            break;
                        case 2:
                            ch = line[j][i];
                            if (isalpha(ch)) { s += ch; state = 2; }
                            else if (isdigit(ch))
                            {
                                s += ch; state = 2;
                            }
                            else
                            {
                                //i--;
                                index = i;
                                return check_reserved(s);
                            }
                            break;

                        case 4:
                            {
                                index = i ;
                                return token.equal_sy;
                            }
                            break;

                        case 3:
                            {
                                index = i ;
                                return token.minus_sy;
                            }
                            break;

                        case 5:
                            ch = line[j][i];
                            if (ch == '=')
                            {

                                index = i+1;
                                return token.greater_equal_sy;
                            }
                            else
                            {
                                //i--;
                                index = i;
                                return token.greater_sy;
                            }
                            break;

                        case 6:
                            ch = line[j][i];
                            if (ch == '=') { index = i + 1; return token.less_equal_sy; }
                            else
                            {
                                //i--;
                                index = i;
                                return token.less_sy;
                            }
                            break;

                        case 7:
                            {
                                index = i;
                                return token.plus_sy;
                            }
                            break;

                        case 8:
                            {
                                index = i ; return token.mult_sy;
                            }
                            break;

                        case 9:
                            {
                                index = i ; return token.lparn_sy;
                            }
                            break;

                        case 10:
                            {
                                index = i ; return token.rparn_sy;
                            }
                            break;

                        case 11:
                            {
                                index = i ; return token.div_sy;
                            }
                            break;

                        case 12:
                            { index = i ; return token.error_sy; }
                            break;


                    }
                   
                }

                index = 0;
                pos = j + 1;

                if (!s.Equals(""))
                {
                    if (isdigit(s[0])) { return token.num_sy; }


                    else if (isalpha(s[0])) { return check_reserved(s); }
                    else if (s[0] == '-') return token.minus_sy;
                    else if (s[0] == '=') return token.equal_sy;
                    else if (s[0] == '<')
                    {
                        if(s.Length>1)
                        {
                            if(s[1]=='=')
                            {
                                return token.less_equal_sy;
                            }
                        }
                        return token.less_sy;
                    }
                    else if (s[0] == '>')
                    {
                        if (s.Length > 1)
                        {
                            if (s[1] == '=')
                            {
                                return token.greater_equal_sy;
                            }
                        }
                        return token.greater_sy;
                    }
                    else if (s[0] == '+') return token.plus_sy;
                    else if (s[0] == '*') return token.mult_sy;
                    else if (s[0] == '(') return token.rparn_sy;
                    else if (s[0] == ')') return token.lparn_sy;
                    else if (s[0] == '/') return token.div_sy;

                }

                
            }
            return token.end_source_sy;
        }
        token check_reserved(string s)
        {
            if (s == "ادخل") return token.enter_sy;
            else if (s == "اطبع") return token.print_sy;
            else if (s == "اذا") return token.if_sy;
            else if (s == "نفذ") return token.then_sy;
            else if (s == "من") return token.from_sy;
            else if (s == "حتي" || s == "حتى") return token.to_sy;
            else if (s == "خطوة") return token.step_sy;
            else if (s == "صحيح") return token.int_sy;
            else if (s == "منطقي" || s == "منطقى") return token.bool_sy;
            else if (s == "نصي" || s == "نصى") return token.string_sy;
            else return token.id;
        }
        public bool isspace(char c)
        {
            return ((c==' ')|| (c == '\n') || (c == '\r') || (c == '\v') || (c == '\f') || (c == '\t'));
        }

        bool isdigit(char c)
        {
            return ((c == '0') || (c == '1') || (c == '2') || (c == '3') || (c == '4') || (c == '5') || (c == '6') || (c == '7') || (c == '8') || (c == '9'));
        }
        bool isalpha(char c)
        {

            return ((c == 'ا') || (c == 'ب') || (c == 'ت') || (c == 'ث') || (c == 'ج') || (c == 'ح') || (c == 'خ') || (c == 'د') || (c == 'ذ') || (c == 'ر') || (c == 'ز') || (c == 'س') || (c == 'ش') || (c == 'ص') || (c == 'ض') || (c == 'ط') || (c == 'ظ') || (c == 'ع') || (c == 'غ') || (c == 'ف') || (c == 'ق') || (c == 'ك') || (c == 'ل') || (c == 'م') || (c == 'ن') || (c == 'ة') || (c == 'و') || (c == 'ي') || (c == 'ى'));
        }


        void match(token terminal)
        {

            if (curr_token == terminal)
                local += name(curr_token) + " تم " + Environment.NewLine;

            else
                syntax_error(curr_token);
            curr_token = get_token();
        }

        void syntax_error(token terminal)
        {
            local += name(terminal) + " غير متوقع "+ Environment.NewLine;
        }
        public void  sample_parser()
        {
            curr_token = get_token();
            program();
            match(token.end_source_sy);

        }
        public string name(token t)
        {
            switch (t)
            {

                case token.bool_sy: return " منطقى "; break;
                case token.div_sy: return " / "; break;
                case token.enter_sy: return " ادخل "; break;
                case token.equal_sy: return " = "; break;
                case token.error_sy: return " خطا "; break;
                case token.from_sy: return " من "; break;
                case token.greater_equal_sy: return " >= "; break;
                case token.greater_sy: return " > "; break;
                case token.id: return " متغير "; break;
                case token.if_sy: return " اذا "; break;
                case token.int_sy: return " صحيح "; break;
                case token.less_equal_sy: return " <= "; break;
                case token.less_sy: return " < "; break;
                case token.lparn_sy: return " ) "; break;
                case token.minus_sy: return " - "; break;
                case token.mult_sy: return " * "; break;
                case token.num_sy: return " ارقام "; break;
                case token.plus_sy: return " + "; break;
                case token.print_sy: return " اطبع "; break;
                case token.rparn_sy: return " ( "; break;
                case token.step_sy: return " خطوة "; break;
                case token.string_sy: return " نصى "; break;
                case token.then_sy: return " نفذ "; break;
                case token.to_sy: return " حتى "; break;
                case token.end_source_sy: return " النهاية "; break;
            }

            return " خطا ";
        }
        //  البرنامج التعريفات الجمل
        public void program()
        {
            variables();
            statment();
        }
        //التعريفات نوع المتغير اسم متغير
        public void variables()
        {
            varType();
            varName();
        }
        public void varType()
        {
            if (curr_token == token.int_sy)
            {
                match(token.int_sy);
            }
            else if (curr_token == token.bool_sy)
            {
                match(token.int_sy);
            }
            else if (curr_token == token.string_sy)
            {
                match(token.string_sy);
            }
            else
            {
                syntax_error(curr_token);
            }
        }
        public void varName()
        {
            match(token.id);
        }
        //الجمل (الجملة*
        public void statment()
        {

            stmt();
            while (curr_token == token.print_sy || curr_token == token.enter_sy || curr_token == token.id || curr_token == token.if_sy || curr_token == token.from_sy)
                stmt();

        }
        public void stmt()
        {
            if (curr_token == token.if_sy)
            {
                //جمله شرطيه
                cond_stmt();
            }
            else if (curr_token == token.from_sy)
            {
                loop_stmt();
            }
            else if (curr_token == token.id)
            {
                assign_stmt();
            }
            else if (curr_token == token.enter_sy)
            {
                enter_stmt();
            }
            else if (curr_token == token.print_sy)
            {
                print_stmt();
            }
            else
            {
                syntax_error(curr_token);
            }
        }
        public void cond_stmt()
        {
            if (curr_token == token.if_sy)
            {
                match(token.if_sy);
                match(token.lparn_sy);
                //تعبير
                exp();
                match(token.rparn_sy);
                match(token.then_sy);
                statment();
            }
            else
            {
                syntax_error(curr_token);
            }
        }
        //جملة اسناد متغير=تعبير

        public void assign_stmt()
        {
            if (curr_token == token.id)
            {
                match(token.id);
                match(token.equal_sy);
                exp();
            }
            else
            {
                syntax_error(curr_token);
            }
        }

        //جملة ادخال ادخل متغير

        public void enter_stmt()
        {
            if (curr_token == token.enter_sy)
            {
                match(token.enter_sy);
                match(token.id);
            }
            else
            {
                syntax_error(curr_token);
            }

        }

        //جملة تكرار من متغير=تعبير حتى تعبير خطوة أرقام نفذ الجمل
        public void loop_stmt()
        {
            if (curr_token == token.from_sy)
            {
                match(token.from_sy);
                match(token.id);
                match(token.equal_sy);
                exp();
                match(token.to_sy);
                exp();
                match(token.step_sy);
                match(token.num_sy);
                match(token.then_sy);
                statment();
            }
            else
            {
                syntax_error(curr_token);
            }
        }
        //جملة طباعة اطبع متغير

        public void print_stmt()
        {
            if (curr_token == token.print_sy)
            {
                match(token.print_sy);
                match(token.id);
            }
            else
            {
                syntax_error(curr_token);
            }
        }

        //تعبير معامل 1عملية1
        public void exp()
        {
            //معامل1
            factor1();
            //عمليه1
            operation1();
        }

        //عملية)=<|<|=>|>( 1معامل 1عمليةƐ 

        public void operation1()
        {
            if (curr_token == token.greater_equal_sy || curr_token == token.less_equal_sy || curr_token == token.greater_sy || curr_token == token.less_sy)
            {

                switch (curr_token)
                {
                    case token.less_equal_sy:
                        match(token.less_equal_sy);
                        break;

                    case token.less_sy:
                        match(token.less_sy);
                        break;

                    case token.greater_equal_sy:
                        match(token.greater_equal_sy);
                        break;

                    case token.greater_sy:
                        match(token.greater_sy);
                        break;
                }
                factor1();
                operation1();
            }
            else
            {
                return;
            }
        }

        //معامل 1معامل 2عملية2
        public void factor1()
        {
            factor2();
            operation2();
        }

        //عملية)-|+( 2معامل 2عمليةƐ | 
        public void operation2()
        {
            if (curr_token == token.plus_sy || curr_token == token.minus_sy)
            {
                switch (curr_token)
                {
                    case token.plus_sy:
                        match(token.plus_sy);
                        break;
                    case token.minus_sy:
                        match(token.minus_sy);
                        break;
                }
                factor2();
                operation2();
            }
            else
            {
                return;
            }
        }

        //معامل 2معامل 3عملية3

        public void factor2()
        {
            factor3();
            operation3();
        }

        //عملية)/|*( 3معامل 3عمليةƐ | 

        public void operation3()
        {
            if (curr_token == token.mult_sy || curr_token == token.div_sy)
            {
                switch (curr_token)
                {
                    case token.mult_sy:
                        match(token.mult_sy);
                        break;
                    case token.div_sy:
                        match(token.div_sy);
                        break;
                }
                factor3();
                operation3();
            }
            else
            {
                return;
            }
        }

        //معامل 3متغير|أرقام

        public void factor3()
        {
            if (curr_token == token.id)
                match(token.id);
            else if (curr_token == token.num_sy)
                match(token.num_sy);
            else
            {
                syntax_error(curr_token);
            }
        }
    }

}
