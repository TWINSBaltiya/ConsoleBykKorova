﻿using System;
using System.Linq;

namespace ConsoleBykKorova
{
    class Program
    {
        const int SEQ = 4; // Размер угадываемой последовательности. Всегда 4.
        const string vers = "1.0"; // Текущая версия игры.
        const string razdel = "***************************************************************************************************************";

        static void Main(string[] args)
        {
            // Шапка с названием и описанием.
            Console.Write("\nИгра \"Быки и коровы\" (версия "+ vers + ").\n");
            Console.Write("Правила игры:\nигрок должен угадать расположение и значение четырех целых чисел;\n");
            Console.Write("в каждый круг игрок вводит 4 целых числа, а система выдает ответ с указанием количества \"Быков\" и \"Коров\";\n");
            Console.Write("\"Бык\" - это совпадение чила и места, \"Корова\" - это совпадение только числа.\n");
            Console.Write(razdel + "\n\n");

            char[] seq_game = new char[SEQ]; // Последовательность для угадывания.
            seq_game.Initialize(); // Инициализируется значением по умолчанию '\0'.
            char[] seq_player = new char[SEQ]; // Последовательность игрока для угадывания инициализируется -1 (далее вводиться игроком).
            seq_player.Initialize(); // Инициализируется значением по умолчанию '\0'.

            bool igra = true; // Логический маркер, что требуется новая последовательность.
            bool vvod = true; // Маркер корректности ввода.
            int byk = 0; // Количество "быков".
            int korova = 0; // Количество "коров".
            int step = 0; // Количество шагов, за которое игрок угодал последовательность.

            Console.Write("Для продолжения введите любой символ или q для выхода: ");

            char quite_game; // Флаг выхода из игры
            quite_game = (char)Console.Read(); // Читает один символ из входного потока в виде его целого (int) численного кода, при этом сразу возможно привести к типу char.

            // Игрок отказался играть.
            if (quite_game == 'q')
            {
                Console.Write("Игра окончена по желанию игрока!\n");
                return;
            }
            else
            {
                Console.Write("\n");
            }

            // Вечный цикл игры.

            while(true)
            {
                if (igra)
                {
                    Random rd = new Random(); // Генератор случайных целых чисел.

                    for (int i = 0; i < seq_game.Count(); i++)
                    {
                        seq_game[i] = rd.Next(0, 9).ToString()[0]; // Возвращает случайное число из диапазона от 0 до 9 (сразу int переводим в char).
                        for (int j = 0; j < i; ++j)
                        {
                            if (seq_game[i] == seq_game[j]) { --i; break; } // Игнорируем повтор цифры (это баг - не очень хорошо менять индекс здесь!).
                        }
                    }

                    Console.Write("\nНовая последовательность из четырех цифр изготовлена! Попробуем угадать!\n");
                    Console.Write("Введите Ваш вариант (четыре цифры от 0 до 9, 'q' для выхода).\n\n");

                    // Последовательность сгенерирована, поэтому маркер устанавливаем в false.
                    igra = false;
                }

                byk = 0;
                korova = 0;

                while ((char)Console.Read() != '\n'); // Очистка буфера ввода.

                // Считывание ввода игрока и сразу подсчет количества Быков и Коров.
                for (int i = 0; i < seq_player.Count(); ++i)
                {
                    // Читаем один символ.
                    seq_player[i] = (char)Console.Read();

                    // Проверка символа на желанеи игрока выйти из игры.
                    if(seq_player[i] == 'q')
                    {
                        Console.Write("Игра окончена по желанию игрока!\n");
                        return; // Экстренный выход игрока из игры.
                    }

                    // Проверка символа на корректность ввода (если ввели не целое число).
                    if(!char.IsNumber(seq_player[i]))
                    {
                        Console.Write("Не верный формат! Повторите ввод: \n");

                        // Очистка - возврат к начальному состоянию для ввода игроком новой последовательности.
                        for(int j = 0; j < seq_player.Count(); j++) seq_player[j] = '\0';
                        byk = 0;
                        korova = 0;

                        // Маркер того, что требуется новый ввод без вывода результата.
                        vvod = false;

                        // Экстренный выход из цикла for.
                        break;
                    }

                    // Маркер того, что новый ввод не нужен и можно вывести результат.
                    vvod = true;


                    // Основная логика подсчета "быков" и "коров".
                    if (seq_game[i] == seq_player[i]) ++byk;
                    else
                    {
                        for (int k1 = 0; k1 < i; ++k1) if (seq_game[k1] == seq_player[i]) ++korova;
                        for (int k2 = i + 1; k2 < seq_game.Count(); ++k2) if (seq_game[k2] == seq_player[i]) ++korova;
                    }
                }

                // Условие окончание тура - это количество быков равно 4 (SEQ).
                // Проверяем это условие после каждого ввода пользователя перед выдачей ответа о "быках" и "коровах".
                // Если верно, то - победа!
                if (byk == SEQ)
                {
                    // Очистка потока
                    while ((char)Console.Read() != '\n') ;

                    Console.BackgroundColor = ConsoleColor.Red;

                    Console.Write("\n" + razdel);
                    Console.Write("\nВаш ввод - правильный: ");
                    for (int j = 0; j < seq_player.Count(); ++j) Console.Write(seq_player[j]);
                    Console.Write("\nИсходная последовательность: ");
                    for (int j = 0; j < seq_game.Count(); ++j) Console.Write(seq_game[j]);
                    ++step;
                    Console.Write("\nВы сделали " + step.ToString() + " шагов.\n");
                    Console.Write("\nПОБЕДА!\n");
                    Console.Write(razdel);

                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.Write("\n\nХотите повторить?\n");
                    Console.Write("Для повтора введите любой символ или 'q' для выхода: ");

                    quite_game = (char)Console.Read(); 

                    // Игрок отказался играть.
                    if (quite_game == 'q')
                    {
                        Console.Write("Игра окончена по желанию игрока!\n");
                        return; // Экстренный выход из игры.
                    }
                    else
                    {
                        Console.Write("\n");
                    }

                    igra = true; // Требуется новая генерация последовательности чисел компьютером.

                    // Очистка массивов символов для новой игры.
                    for (int i = 0; i < seq_game.Count(); ++i)
                    {
                        seq_game[i] = '\0';
                        seq_player[i] = '\0';
                    }

                    // Обнуление счетчика шагов.
                    step = 0;

                    // Переход к новой итерации бесконечного основного цикла для новой игры.
                    continue;
                }

                // Вывод результата, если ввод корректен.
                if (vvod)
                {
                    // Двигаем курсор, чтобы ответ был на той же строчке, что и ввод игрока.
                    Console.CursorTop--; // На строку назад
                    Console.CursorLeft = 7; // на 7 позиций от начала вправо.

                    Console.Write('Б' + byk.ToString() + "  " + 'К' + korova.ToString() + "\n");

                    // Увеличиваем счетчик шагов.
                    ++step;
                }
            }

        }
    }
}
