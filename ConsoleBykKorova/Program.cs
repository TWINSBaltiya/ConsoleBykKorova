using System;
using System.Linq;

namespace ConsoleBykKorova
{
    class Program
    {
        #region const
        const int SEQ = 4; // Размер угадываемой последовательности. Всегда 4.
        const string vers = "1.1"; // Текущая версия игры.
        const string razdel = "***************************************************************************************************************";

        #endregion

        static void Main(string[] args)
        {
            #region pravila
            // Шапка с названием и описанием.
            Console.Write("\nИгра \"Быки и коровы\" (версия "+ vers + ").\n");
            Console.Write("Правила игры:\nигрок должен угадать расположение и значение четырех целых чисел;\n");
            Console.Write("в каждый круг игрок вводит 4 целых числа, а система выдает ответ с указанием количества \"Быков\" и \"Коров\";\n");
            Console.Write("\"Бык\" - это совпадение чила и места, \"Корова\" - это совпадение только числа.\n");
            Console.Write(razdel + "\n\n");

            Console.Write("Для продолжения введите любой символ или q для выхода: ");
            #endregion

            #region variable
            char[] seq_game = new char[SEQ]; // Последовательность для угадывания.
            char[] seq_player = new char[SEQ]; // Последовательность игрока для угадывания.

            bool igra = true; // Логический маркер, что требуется новая последовательность.
            bool vvod = true; // Маркер корректности ввода.
            int byk = 0; // Количество "быков".
            int korova = 0; // Количество "коров".
            int step = 0; // Количество шагов, за которое игрок угодал последовательность.

            char quite_game; // Флаг выхода из игры
            #endregion

            #region quitting the game
            // Читает один символ из входного потока в виде его целого (int) численного кода, при этом сразу возможно привести к типу char.
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
            #endregion
          
            // Вечный цикл игры.
            while (true)
            {
                #region random 
                if (igra)
                {
                    Random rd = new Random(); // Генератор случайных целых чисел.

                    for (int i = 0; i < seq_game.Count(); i++)
                    {
                        // Возвращает случайное число из диапазона от 0 до 9 (сразу int переводим в char).
                        seq_game[i] = rd.Next(0, 10).ToString()[0];
                        for (int j = 0; j < i; ++j)
                        {
                            // Игнорируем повтор цифры (это баг - не очень хорошо менять индекс здесь!).
                            if (seq_game[i] == seq_game[j]) { --i; break; }
                        }
                    }

                    // Последовательность сгенерирована, поэтому маркер устанавливаем в false.
                    igra = false;

                    // Приглашеение к игре.
                    Console.Write("\n");
                    Console.Write("Новая последовательность из четырех цифр изготовлена! Попробуем угадать!\n");
                    Console.Write("Введите Ваш вариант (четыре цифры от 0 до 9, 'q' для выхода).\n\n");

                    // Первоначальная очистка буфера ввода - просто считываем в никуда все, что есть.
                    while ((char)Console.Read() != '\n') ;
                }
                #endregion

                // Обнуляем контролируемые значения.
                byk = 0;
                korova = 0;
                vvod = true;

                // Считывание ввода игрока.
                string strline = Console.ReadLine();

                // Определяем был ли ввод полным.
                int minindex = seq_player.Count() > strline.Length ? strline.Length : seq_player.Count();

                for (int i = 0; i < minindex; ++i)
                {
                    #region verification
                    // Читаем один символ из загруженной строки.
                    seq_player[i] = strline[i];

                    // Проверка символа на желанеи игрока выйти из игры.
                    if (seq_player[i] == 'q')
                    {
                        Console.Write("Игра окончена по желанию игрока!\n");
                        return; // Экстренный выход игрока из игры.
                    }

                    // Проверка на полноту ввода.
                    if(minindex < seq_player.Count()) vvod = false;

                    // Проверка символа на корректность ввода.
                    if(!char.IsNumber(seq_player[i])) vvod = false;

                    // Проверка на повторение цифр.
                    for(int k = 0; k < i; k++)
                    {
                        if(seq_player[k] == seq_player[i])
                        {
                            vvod = false;

                            // Выход из текущего цикла for.
                            break;
                        }
                    }

                    // При некорректном вводе - возврат к начальному состоянию для ввода игроком новой последовательности.
                    if(!vvod)
                    {
                        // Двигаем курсор на прежнее место, чтобы очистить неверный ввод.
                        Console.CursorTop--;
                        Console.CursorLeft = 0;

                        // Очистка.
                        for(int j = 0; j < strline.Count(); j++)
                        {
                            Console.Write(' ');
                        }

                        // Опять возвращаем курсор в начало строки для нового ввода.
                        Console.CursorLeft = 0;

                        // Очистка последовательности игрока для ввода игроком новой последовательности.
                        for (int j = 0; j < seq_player.Count(); j++) seq_player[j] = '\0';

                        // Возврат к начальному состоянию.
                        byk = 0;
                        korova = 0;

                        // Экстренный выход из цикла for, анализирующего символы, - для нового ввода.
                        break;
                    }                   
                    #endregion

                    #region logic
                    // Маркер того, что новый ввод не нужен и можно вывести результат.
                    vvod = true;

                    // Основная логика подсчета "быков" и "коров".
                    if (seq_game[i] == seq_player[i]) ++byk;
                    else
                    {
                        for (int k1 = 0; k1 < i; ++k1) if (seq_game[k1] == seq_player[i]) ++korova;
                        for (int k2 = i + 1; k2 < seq_game.Count(); ++k2) if (seq_game[k2] == seq_player[i]) ++korova;
                    }
                    #endregion
                }

                #region victory
                // Условие окончание тура - это равенство количества быков 4 (SEQ).
                // Проверяем это условие после каждого ввода пользователя перед выдачей ответа о "быках" и "коровах".
                // Если верно, то - победа!
                if (byk == SEQ)
                {
                    // Очистка входного потока - просто считываем в никуда все, что осталось.
                    // while ((char)Console.Read() != '\n') ;

                    // Подкрашиваем фон в красный цвет для выделения поздравления.
                    Console.BackgroundColor = ConsoleColor.Red;

                    Console.Write("\n" + razdel);
                    Console.Write("\nВаш ввод - правильный: ");
                    for (int j = 0; j < seq_player.Count(); ++j) Console.Write(seq_player[j]);
                    Console.Write("\nИсходная последовательность: ");
                    for (int j = 0; j < seq_game.Count(); ++j) Console.Write(seq_game[j]);
                    ++step;
                    Console.Write("\nВы сделали " + step.ToString() + " шагов.\n");
                    Console.Write("ПОБЕДА!\n");
                    Console.Write(razdel);

                    // Возвращаем цвет фона к черному.
                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.Write("\n\n");
                    Console.Write("Хотите повторить?\n");
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

                    // СИгнализируем, что требуется новая генерация последовательности чисел компьютером.
                    igra = true;

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
                #endregion

                #region result
                // Вывод результата, если ввод корректен, но еще не победа.
                if (vvod)
                {
                    // Двигаем курсор, чтобы ответ был на той же строчке, что и ввод игрока.
                    Console.CursorTop--; // На строку назад
                    Console.CursorLeft = SEQ; // Выставляем курсор после введенных цифр.

                    for (int j = 0; j < strline.Count() - SEQ; j++)
                    {
                        Console.Write(' ');
                    }

                    Console.CursorLeft = SEQ*2; // на 9 позиций от начала вправо (индексация начинается с 0).

                    Console.Write('Б' + byk.ToString() + "  " + 'К' + korova.ToString() + "\n");

                    // Увеличиваем счетчик шагов.
                    ++step;
                }
                #endregion
            }
        }
    }
}
