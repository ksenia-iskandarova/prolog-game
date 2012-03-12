using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WFProlog
{
    
    public partial class Form1 : Form
    {
        string dll = "";
        public Form1()
        {
           
            Logic.getdelete_db();
            InitializeComponent();
        }
        List<int[]> mas = new List<int[]>();
        private void button1_Click(object sender, EventArgs e)
        {
            if (dll == "final.dll")
            {
                bool flag = false;
                int[] m1;
                int[] m2 = new int[2];
                int numx0 = 4;
                int numy0 = 3;
                int xxx = 0;
                //заносим нулевой ход
                xxx = Logic.getsay(numx0, numy0);
                m1 = new int[2];
                int[] tmp = new int[3];
                m1[0] = numx0; m1[1] = numy0;

                //считываем ход человека
                int numx = Int32.Parse(textBox1.Text);
                int numy = Int32.Parse(textBox3.Text);

                //если этого хода нет в базе
                if (xxx == 0)
                {
                    //добавляем  в список концов нулевую точку
                    mas.Add(m1);
                    textBox5.Text = "";
                    m1 = new int[2];
                    m1[0] = numx; m1[1] = numy;
                    // добавляем в список концов точку ччеловека
                    mas.Add(m1);
                    xxx = Logic.getsay(numx, numy);//Logic.get_hello(8);
                    //проверяем есть ли точка человека в бд
                    if (xxx == 0)
                    {
                        textBox2.Text = "Шаг сделан. Ход компьютера.";
                        textBox5.Text = "";
                        textBox4.Text = "";
                        //делаем пока флаг - фолс
                        do
                        {
                            tmp = Logic.getshift(numx, numy);
                            
                            //запрашиваем координаты компьютера
                            int numk1x = mas.ElementAt(0)[0];
                            int numk1y = mas.ElementAt(0)[1];
                            int razn1x = Math.Abs(tmp[0] - numk1x);
                            int razn1y = Math.Abs(tmp[1] - numk1y);
                            //проверяем начинается ли он с начального конца
                            if (razn1x == 1 && razn1y == 0 || razn1x == 0 && razn1y == 1)
                            {
                                //если да то флаг - тру
                                flag = true;
                                textBox4.Text = tmp[0].ToString();
                                textBox5.Text = tmp[1].ToString();
                                //проверяем есть ли эта точка в бд
                                tmp[2] = Logic.getsay(tmp[0], tmp[1]);
                                m2[0] = tmp[0];
                                m2[1] = tmp[1];
                                //заносим в список концов
                                mas.Add(m2);
                                textBox6.Text = tmp[2].ToString();
                                textBox2.Text = "Шаг сделан. Ход человека.";
                            }
                            //проверяем начинается ли он с конечного конца
                            int numk2x = mas.ElementAt(mas.Count - 1)[0];
                            int numk2y = mas.ElementAt(mas.Count - 1)[1];
                            int razn2x = Math.Abs(tmp[0] - numk2x);
                            int razn2y = Math.Abs(tmp[1] - numk2y);
                            if (razn2x == 1 && razn2y == 0 || razn2x == 0 && razn2y == 1)
                            {
                                //если да то флаг - тру
                                flag = true;
                                textBox4.Text = tmp[0].ToString();
                                textBox5.Text = tmp[1].ToString();
                                tmp[2] = Logic.getsay(tmp[0], tmp[1]);
                                //проверяем есть ли в бд
                                m2[0] = tmp[0];
                                m2[1] = tmp[1];
                                //заносим в список концов
                                mas.Add(m2);
                                textBox6.Text = tmp[2].ToString();
                                textBox2.Text = "Шаг сделан. Ход человека.";
                                //textBox6.Text = tmp[2].ToString();
                            }

                        } while (flag == false);
                    }
                }
                //еслиначальная точка уже есть в бд
                if (xxx == 1)
                {
                    flag = false;
                    bool flag2 = false;
                    //проверяем точка человека с начального конца?
                    int numk1x = mas.ElementAt(0)[0];
                    int numk1y = mas.ElementAt(0)[1];
                    int razn1x = Math.Abs(numx - numk1x);
                    int razn1y = Math.Abs(numy - numk1y);
                    if ((razn1x == 1 && razn1y == 0) || (razn1x == 0 && razn1y == 1))
                    {
                        //если да то флаг -тру
                        m1 = new int[2];
                        m1[0] = numx; m1[1] = numy;
                        flag = true;
                        //вставляем в начало списка концов
                        mas.Insert(0, m1);
                        //проевряем попали ли на прямую
                        xxx = Logic.getsay(numx, numy);
                        if (xxx == 1)
                            //если попали - то выигрыш
                            textBox2.Text = "Выигрыш!";
                        if (xxx == 0)
                        {
                            //если не попали то делаем пока флаг фолс
                            do
                            {
                                //считываем координату компа
                                tmp = Logic.getshift(numx, numy);
                                //проверяем есть ли она в бд
                                tmp[2] = Logic.getsay(tmp[0], tmp[1]);

                                int[] check = new int[2];
                                check[0] = tmp[0];
                                check[1] = tmp[1];
                                //проверяем чтоб этот ход не был ходом назад
                                //если не ход назад и мы попали на линию то выигрыш
                                if (tmp[2] == 1)
                                {
                                    if ((mas[1][0] != check[0] || mas[1][1] != check[1]) && (mas[mas.Count - 2][0] != check[0] || mas[mas.Count - 2][1] != check[1]))
                                    { textBox2.Text = "Компьютер выиграл"; flag2 = true; }
                                    if (mas[0][0] == check[0] && mas[0][1] == check[1] || mas[mas.Count - 1][0] == check[0] && mas[mas.Count - 1][1] == check[0])
                                    { textBox2.Text = "Компьютер выиграл"; flag2 = true; }
                                }
                                if (tmp[2] == 1)
                                    if ((mas[1][0] == check[0] && mas[1][1] == check[1]) || (mas[mas.Count - 2][0] == check[0] && mas[mas.Count - 2][1] == check[1]))
                                    {
                                        tmp = Logic.getnonshift(numx, numy);
                                        tmp[2] = Logic.getsay(tmp[0], tmp[1]);
                                    }
                                //если нет
                                if (tmp[2] == 0)
                                {
                                    //проверяем с начального ли конца пошел компьютер
                                    int numk3x = mas.ElementAt(0)[0];
                                    int numk3y = mas.ElementAt(0)[1];
                                    int razn3x = Math.Abs(tmp[0] - numk3x);
                                    int razn3y = Math.Abs(tmp[1] - numk3y);
                                    if (razn3x == 1 && razn3y == 0 || razn3x == 0 && razn3y == 1)
                                    {
                                        //если да - то флаг тру
                                        flag2 = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        //заносим  точку в бд
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        //заносим в начало списка концов
                                        mas.Insert(0, m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";

                                    }
                                    //проверяем пошел ли комп с конечного конца
                                    int numk4x = mas.ElementAt(mas.Count - 1)[0];
                                    int numk4y = mas.ElementAt(mas.Count - 1)[1];
                                    int razn4x = Math.Abs(tmp[0] - numk4x);
                                    int razn4y = Math.Abs(tmp[1] - numk4y);
                                    if (razn4x == 1 && razn4y == 0 || razn4x == 0 && razn4y == 1)
                                    {
                                        //если да - то флаг тру
                                        flag2 = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        //заносим в конец списка концов
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        mas.Insert(mas.Count, m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";

                                    }
                                }
                            } while (flag2 == false);
                        }
                    }

                    if (flag == false)
                    {

                        int numk2x = mas.ElementAt(mas.Count - 1)[0];
                        int numk2y = mas.ElementAt(mas.Count - 1)[1];
                        int razn2x = Math.Abs(numx - numk2x);
                        int razn2y = Math.Abs(numy - numk2y);
                        if (razn2x == 1 && razn2y == 0 || razn2x == 0 && razn2y == 1)
                        {
                            flag = true;
                            flag2 = false;
                            m1 = new int[2];
                            m1[0] = numx; m1[1] = numy;
                            mas.Insert(mas.Count, m1);
                            xxx = Logic.getsay(numx, numy);//Logic.get_hello(8);
                            if (xxx == 1)
                                textBox2.Text = "Выигрыш!";
                            if (xxx == 0)
                            {
                                do
                                {
                                    tmp = Logic.getshift(numx, numy);
                                    tmp[2] = Logic.getsay(tmp[0], tmp[1]);

                                    int[] check = new int[2];
                                    check[0] = tmp[0];
                                    check[1] = tmp[1];
                                    if (tmp[2] == 1)
                                        if ((mas[1][0] != check[0] && mas[1][1] != check[1]) && (mas[mas.Count - 2][0] != check[0] && mas[mas.Count - 2][1] != check[1]))
                                            textBox2.Text = "Компьютер выиграл";

                                    if (tmp[2] == 1)
                                        if ((mas[1][0] == check[0] && mas[1][1] == check[1]) || (mas[mas.Count - 2][0] == check[0] && mas[mas.Count - 2][1] == check[1]))
                                        {
                                            tmp = Logic.getnonshift(numx, numy);
                                            tmp[2] = Logic.getsay(tmp[0], tmp[1]);
                                        }


                                    if (tmp[2] == 0)
                                    {
                                        int numk3x = mas.ElementAt(0)[0];
                                        int numk3y = mas.ElementAt(0)[1];
                                        int razn3x = Math.Abs(tmp[0] - numk3x);
                                        int razn3y = Math.Abs(tmp[1] - numk3y);
                                        if (razn3x == 1 && razn3y == 0 || razn3x == 0 && razn3y == 1)
                                        {
                                            flag2 = true;
                                            textBox4.Text = tmp[0].ToString();
                                            textBox5.Text = tmp[1].ToString();
                                            tmp[2] = Logic.getsay(tmp[0], tmp[1]);

                                            int[] checks = new int[2];
                                            checks[0] = tmp[0];
                                            checks[1] = tmp[1];
                                            if (tmp[2] == 1)
                                                if ((mas[1][0] != checks[0] && mas[1][1] != checks[1]) || (mas[mas.Count - 2][0] != checks[0] && mas[mas.Count - 2][1] != check[1]))
                                                    textBox2.Text = "Компьютер выиграл";

                                            if (tmp[2] == 1)
                                                if ((mas[1][0] == checks[0] && mas[1][1] == checks[1]) || (mas[mas.Count - 2][0] == checks[0] && mas[mas.Count - 2][1] == check[1]))
                                                {
                                                    tmp = Logic.getnonshift(numx, numy);
                                                    tmp[2] = Logic.getsay(tmp[0], tmp[1]);
                                                }


                                            if (tmp[2] == 0)
                                            {
                                                mas.Insert(0, m2);
                                                m2[0] = tmp[0];
                                                m2[1] = tmp[1];

                                                textBox6.Text = tmp[2].ToString();
                                                textBox2.Text = "Шаг сделан. Ход человека.";
                                            }
                                        }

                                        int numk4x = mas.ElementAt(mas.Count - 1)[0];
                                        int numk4y = mas.ElementAt(mas.Count - 1)[1];
                                        int razn4x = Math.Abs(tmp[0] - numk4x);
                                        int razn4y = Math.Abs(tmp[1] - numk4y);
                                        if (razn4x == 1 && razn4y == 0 || razn4x == 0 && razn4y == 1)
                                        {

                                            flag2 = true;
                                            textBox4.Text = tmp[0].ToString();
                                            textBox5.Text = tmp[1].ToString();
                                            tmp[2] = Logic.getsay(tmp[0], tmp[1]);

                                            m2[0] = tmp[0];
                                            m2[1] = tmp[1];
                                            mas.Insert(mas.Count, m2);
                                            //mas.Add(m2);
                                            textBox6.Text = tmp[2].ToString();
                                            textBox2.Text = "Шаг сделан. Ход человека.";
                                            //textBox6.Text = tmp[2].ToString();

                                        }
                                    }
                                } while (flag2 == false);
                            }
                        }
                        else
                        {
                            textBox2.Text = "Невозможно сходить.";
                        }
                    }

                }

            }
            if (dll == "test2.dll")
            {
                bool flag = false;
                int[] m1;
                int[] m2 = new int[2];
                int numx0 = 4;
                int numy0 = 3;
                int xxx = 0;
                //заносим нулевой ход
                xxx = Logic.getsay2(numx0, numy0);
                m1 = new int[2];
                int[] tmp = new int[3];
                m1[0] = numx0; m1[1] = numy0;

                //считываем ход человека
                int numx = Int32.Parse(textBox1.Text);
                int numy = Int32.Parse(textBox3.Text);

                //если этого хода нет в базе
                if (xxx == 0)
                {
                    //добавляем  в список концов нулевую точку
                    mas.Add(m1);
                    textBox5.Text = "";
                    m1 = new int[2];
                    m1[0] = numx; m1[1] = numy;
                    // добавляем в список концов точку ччеловека
                    mas.Add(m1);
                    xxx = Logic.getsay2(numx, numy);//Logic.get_hello(8);
                    //проверяем есть ли точка человека в бд
                    if (xxx == 0)
                    {
                       
                        textBox2.Text = "Шаг сделан. Ход компьютера.";
                        textBox5.Text = "";
                        textBox4.Text = "";
                        //делаем пока флаг - фолс
                        do
                        {
                            tmp = Logic.getshift2(numx, numy);
                            //запрашиваем координаты компьютера
                            tmp[2] = Logic.getsay2(tmp[0], tmp[1]);

                                int[] check = new int[2];
                                check[0] = tmp[0];
                                check[1] = tmp[1];
                                //проверяем чтоб этот ход не был ходом назад
                                //если не ход назад и мы попали на линию то выигрыш
                                if (tmp[2] == 1)
                                {
                                    if ((mas[1][0] != check[0] || mas[1][1] != check[1]) && (mas[mas.Count - 2][0] != check[0] || mas[mas.Count - 2][1] != check[1]))
                                    { textBox2.Text = "Компьютер выиграл"; }
                                    if (mas[0][0] == check[0] && mas[0][1] == check[1] || mas[mas.Count - 1][0] == check[0] && mas[mas.Count - 1][1] == check[0])
                                    { textBox2.Text = "Компьютер выиграл"; }
                                }
                                if (tmp[2] == 1)
                                    if ((mas[1][0] == check[0] && mas[1][1] == check[1]) || (mas[mas.Count - 2][0] == check[0] && mas[mas.Count - 2][1] == check[1]))
                                    {
                                        tmp = Logic.getnonshift2(numx, numy);
                                        tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                    }
                                //если нет
                                if (tmp[2] == 0)
                                {
                                    int numk1x = mas.ElementAt(0)[0];
                                    int numk1y = mas.ElementAt(0)[1];
                                    int razn1x = Math.Abs(tmp[0] - numk1x);
                                    int razn1y = Math.Abs(tmp[1] - numk1y);
                                    //проверяем начинается ли он с начального конца
                                    if (razn1x == 1 && razn1y == 0 || razn1x == 0 && razn1y == 1)
                                    {
                                        //если да то флаг - тру
                                        flag = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        //проверяем есть ли эта точка в бд
                                        tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        //заносим в список концов
                                        mas.Add(m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";
                                    }
                                    //проверяем начинается ли он с конечного конца
                                    int numk2x = mas.ElementAt(mas.Count - 1)[0];
                                    int numk2y = mas.ElementAt(mas.Count - 1)[1];
                                    int razn2x = Math.Abs(tmp[0] - numk2x);
                                    int razn2y = Math.Abs(tmp[1] - numk2y);
                                    if (razn2x == 1 && razn2y == 0 || razn2x == 0 && razn2y == 1)
                                    {
                                        //если да то флаг - тру
                                        flag = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                        //проверяем есть ли в бд
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        //заносим в список концов
                                        mas.Add(m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";
                                        //textBox6.Text = tmp[2].ToString();
                                    }
                                }
                        } while (flag == false);
                    }
                }
                //еслиначальная точка уже есть в бд
                if (xxx == 1)
                {
                    flag = false;
                    bool flag2 = false;
                    //проверяем точка человека с начального конца?
                    int numk1x = mas.ElementAt(0)[0];
                    int numk1y = mas.ElementAt(0)[1];
                    int razn1x = Math.Abs(numx - numk1x);
                    int razn1y = Math.Abs(numy - numk1y);
                    if ((razn1x == 1 && razn1y == 0) || (razn1x == 0 && razn1y == 1))
                    {
                        //если да то флаг -тру
                        m1 = new int[2];
                        m1[0] = numx; m1[1] = numy;
                        flag = true;
                        //вставляем в начало списка концов
                        mas.Insert(0, m1);
                        //проевряем попали ли на прямую
                        xxx = Logic.getsay2(numx, numy);
                        if (xxx == 1)
                            //если попали - то выигрыш
                            textBox2.Text = "Выигрыш!";
                        if (xxx == 0)
                        {
                            //если не попали то делаем пока флаг фолс
                            do
                            {
                                //считываем координату компа
                                tmp = Logic.getshift2(numx, numy);
                                //проверяем есть ли она в бд
                                tmp[2] = Logic.getsay2(tmp[0], tmp[1]);

                                int[] check = new int[2];
                                check[0] = tmp[0];
                                check[1] = tmp[1];
                                //проверяем чтоб этот ход не был ходом назад
                                //если не ход назад и мы попали на линию то выигрыш
                                if (tmp[2] == 1)
                                {
                                    if ((mas[1][0] != check[0] || mas[1][1] != check[1]) && (mas[mas.Count - 2][0] != check[0] || mas[mas.Count - 2][1] != check[1]))
                                    { textBox2.Text = "Компьютер выиграл"; flag2 = true; }
                                    if (mas[0][0] == check[0] && mas[0][1] == check[1] || mas[mas.Count - 1][0] == check[0] && mas[mas.Count - 1][1] == check[0])
                                    { textBox2.Text = "Компьютер выиграл"; flag2 = true; }
                                }
                                if (tmp[2] == 1)
                                    if ((mas[1][0] == check[0] && mas[1][1] == check[1]) || (mas[mas.Count - 2][0] == check[0] && mas[mas.Count - 2][1] == check[1]))
                                    {
                                        tmp = Logic.getnonshift2(numx, numy);
                                        tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                    }
                                //если нет
                                if (tmp[2] == 0)
                                {
                                    //проверяем с начального ли конца пошел компьютер
                                    int numk3x = mas.ElementAt(0)[0];
                                    int numk3y = mas.ElementAt(0)[1];
                                    int razn3x = Math.Abs(tmp[0] - numk3x);
                                    int razn3y = Math.Abs(tmp[1] - numk3y);
                                    if (razn3x == 1 && razn3y == 0 || razn3x == 0 && razn3y == 1)
                                    {
                                        //если да - то флаг тру
                                        flag2 = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        //заносим  точку в бд
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        //заносим в начало списка концов
                                        mas.Insert(0, m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";

                                    }
                                    //проверяем пошел ли комп с конечного конца
                                    int numk4x = mas.ElementAt(mas.Count - 1)[0];
                                    int numk4y = mas.ElementAt(mas.Count - 1)[1];
                                    int razn4x = Math.Abs(tmp[0] - numk4x);
                                    int razn4y = Math.Abs(tmp[1] - numk4y);
                                    if (razn4x == 1 && razn4y == 0 || razn4x == 0 && razn4y == 1)
                                    {
                                        //если да - то флаг тру
                                        flag2 = true;
                                        textBox4.Text = tmp[0].ToString();
                                        textBox5.Text = tmp[1].ToString();
                                        //заносим в конец списка концов
                                        m2[0] = tmp[0];
                                        m2[1] = tmp[1];
                                        mas.Insert(mas.Count, m2);
                                        textBox6.Text = tmp[2].ToString();
                                        textBox2.Text = "Шаг сделан. Ход человека.";

                                    }
                                }
                            } while (flag2 == false);
                        }
                    }

                    if (flag == false)
                    {

                        int numk2x = mas.ElementAt(mas.Count - 1)[0];
                        int numk2y = mas.ElementAt(mas.Count - 1)[1];
                        int razn2x = Math.Abs(numx - numk2x);
                        int razn2y = Math.Abs(numy - numk2y);
                        if (razn2x == 1 && razn2y == 0 || razn2x == 0 && razn2y == 1)
                        {
                            flag = true;
                            flag2 = false;
                            m1 = new int[2];
                            m1[0] = numx; m1[1] = numy;
                            mas.Insert(mas.Count, m1);
                            xxx = Logic.getsay2(numx, numy);//Logic.get_hello(8);
                            if (xxx == 1)
                                textBox2.Text = "Выигрыш!";
                            if (xxx == 0)
                            {
                                do
                                {
                                    tmp = Logic.getshift2(numx, numy);
                                    tmp[2] = Logic.getsay2(tmp[0], tmp[1]);

                                    int[] check = new int[2];
                                    check[0] = tmp[0];
                                    check[1] = tmp[1];
                                    if (tmp[2] == 1)
                                        if ((mas[1][0] != check[0] && mas[1][1] != check[1]) && (mas[mas.Count - 2][0] != check[0] && mas[mas.Count - 2][1] != check[1]))
                                            textBox2.Text = "Компьютер выиграл";

                                    if (tmp[2] == 1)
                                        if ((mas[1][0] == check[0] && mas[1][1] == check[1]) || (mas[mas.Count - 2][0] == check[0] && mas[mas.Count - 2][1] == check[1]))
                                        {
                                            tmp = Logic.getnonshift2(numx, numy);
                                            tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                        }


                                    if (tmp[2] == 0)
                                    {
                                        int numk3x = mas.ElementAt(0)[0];
                                        int numk3y = mas.ElementAt(0)[1];
                                        int razn3x = Math.Abs(tmp[0] - numk3x);
                                        int razn3y = Math.Abs(tmp[1] - numk3y);
                                        if (razn3x == 1 && razn3y == 0 || razn3x == 0 && razn3y == 1)
                                        {
                                            flag2 = true;
                                            textBox4.Text = tmp[0].ToString();
                                            textBox5.Text = tmp[1].ToString();
                                            tmp[2] = Logic.getsay2(tmp[0], tmp[1]);

                                            int[] checks = new int[2];
                                            checks[0] = tmp[0];
                                            checks[1] = tmp[1];
                                            if (tmp[2] == 1)
                                                if ((mas[1][0] != checks[0] && mas[1][1] != checks[1]) || (mas[mas.Count - 2][0] != checks[0] && mas[mas.Count - 2][1] != check[1]))
                                                    textBox2.Text = "Компьютер выиграл";

                                            if (tmp[2] == 1)
                                                if ((mas[1][0] == checks[0] && mas[1][1] == checks[1]) || (mas[mas.Count - 2][0] == checks[0] && mas[mas.Count - 2][1] == check[1]))
                                                {
                                                    tmp = Logic.getnonshift2(numx, numy);
                                                    tmp[2] = Logic.getsay2(tmp[0], tmp[1]);
                                                }


                                            if (tmp[2] == 0)
                                            {
                                                mas.Insert(0, m2);
                                                m2[0] = tmp[0];
                                                m2[1] = tmp[1];

                                                textBox6.Text = tmp[2].ToString();
                                                textBox2.Text = "Шаг сделан. Ход человека.";
                                            }
                                        }

                                        int numk4x = mas.ElementAt(mas.Count - 1)[0];
                                        int numk4y = mas.ElementAt(mas.Count - 1)[1];
                                        int razn4x = Math.Abs(tmp[0] - numk4x);
                                        int razn4y = Math.Abs(tmp[1] - numk4y);
                                        if (razn4x == 1 && razn4y == 0 || razn4x == 0 && razn4y == 1)
                                        {

                                            flag2 = true;
                                            textBox4.Text = tmp[0].ToString();
                                            textBox5.Text = tmp[1].ToString();
                                            tmp[2] = Logic.getsay2(tmp[0], tmp[1]);

                                            m2[0] = tmp[0];
                                            m2[1] = tmp[1];
                                            mas.Insert(mas.Count, m2);
                                            //mas.Add(m2);
                                            textBox6.Text = tmp[2].ToString();
                                            textBox2.Text = "Шаг сделан. Ход человека.";
                                            //textBox6.Text = tmp[2].ToString();

                                        }
                                    }
                                } while (flag2 == false);
                            }
                        }
                        else
                        {
                            textBox2.Text = "Невозможно сходить.";
                        }
                    }

                }          
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                dll = "final.dll";
            if (comboBox1.SelectedIndex == 1)
                dll = "test2.dll";


        }
    }
    class Logic
    {
        //public int[] xs;
        //public int[] ys;

        [DllImport("test2.DLL")]
        unsafe private static extern int say2(int x, int y, int* d);
        public unsafe static int getsay2(int x, int y)
        {
            int s;
            say2(x, y, &s);
            return s;
        }

        [DllImport("test2.DLL")]
        unsafe private static extern int delete_db2();
        public unsafe static int getdelete_db2()
        {

            delete_db2();
            return 0;
        }

        [DllImport("test2.DLL")]
        unsafe private static extern int sayy2(int c, int* d);
        public unsafe static int getsayy2(int y)
        {
            int s;
            sayy2(y, &s);
            return s;
        }

        /*[DllImport("final.DLL")]
        unsafe private static extern string getway(string c, string[]k, int *d);
        public unsafe static string get_way(string x,string[]xs)
        {
            int f;
            getway(x, xs, &f);
            string c = "Y = " + f.ToString();
            return c;
        }*/


        [DllImport("test2.DLL")]
        unsafe private static extern int shift2(int x, int y, int* d, int* j);
        public unsafe static int[] getshift2(int x, int y)
        {
            int s, j, k;
            shift2(x, y, &s, &j);
            int[] tmpmas = new int[3];
            tmpmas[0] = s;
            tmpmas[1] = j;
            string c = "X = " + s.ToString() + "Y = " + j.ToString();
            return tmpmas;
        }
        [DllImport("test2.DLL")]
        unsafe private static extern int nonshift2(int x, int y, int* d, int* j);
        public unsafe static int[] getnonshift2(int x, int y)
        {
            int s, j, k;
            nonshift2(x, y, &s, &j);
            int[] tmpmas = new int[3];
            tmpmas[0] = s;
            tmpmas[1] = j;
            string c = "X = " + s.ToString() + "Y = " + j.ToString();
            return tmpmas;
        }




        [DllImport("final.DLL")]
        unsafe private static extern int say(int x,int y, int* d);
        public unsafe static int getsay(int x,int y)
        {
            int s;
            say(x,y, &s);
            return s;
        }

        [DllImport("final.DLL")]
        unsafe private static extern int delete_db();
        public unsafe static int getdelete_db()
        {
           
            delete_db();
            return 0;
        }

        [DllImport("final.DLL")]
        unsafe private static extern int sayy(int c,  int* d);
        public unsafe static int getsayy(int y)
        {
            int s;
            sayy(y,  &s);
            return s;
        }

        /*[DllImport("final.DLL")]
        unsafe private static extern string getway(string c, string[]k, int *d);
        public unsafe static string get_way(string x,string[]xs)
        {
            int f;
            getway(x, xs, &f);
            string c = "Y = " + f.ToString();
            return c;
        }*/


        [DllImport("final.DLL")]
        unsafe private static extern int shift(int x, int y, int* d, int * j);
        public unsafe static int[] getshift(int x, int y)
        {
            int s,j,k;
            shift(x, y, &s, &j);
            int[] tmpmas = new int[3];
            tmpmas[0] = s;
            tmpmas[1] = j;            
            string c = "X = " + s.ToString() + "Y = " + j.ToString();
            return tmpmas;
        }
        [DllImport("final.DLL")]
        unsafe private static extern int nonshift(int x, int y, int* d, int* j);
        public unsafe static int[] getnonshift(int x, int y)
        {
            int s, j, k;
            nonshift(x, y, &s, &j);
            int[] tmpmas = new int[3];
            tmpmas[0] = s;
            tmpmas[1] = j;
            string c = "X = " + s.ToString() + "Y = " + j.ToString();
            return tmpmas;
        }
    }
}
