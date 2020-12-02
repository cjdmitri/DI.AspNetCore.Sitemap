using System;
using System.IO;
using DI.AspNetCore.Sitemap;

namespace DI.AspNetCore.Sitemap.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Создание карты сайта...");
            string path = "sitemap.xml";
            string url = "https://site.ru";
            string url2 = "https://site.ru/blog";
            if (Sitemap.CreateSitemap(path))
            {
                Console.WriteLine("Карта сайта создана");
                Console.WriteLine(File.ReadAllText(path));
            }
            else
            {
                Console.WriteLine("Карта сайта уже существует...");
            }

            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("Добавим элемент в карту сайта");
                if (Sitemap.AddItem(path, url2 + i.ToString(), DateTime.Now, Sitemap.ChangeFrequency.Always, 0.6))
                    Console.WriteLine("Произвольный элемент добавлен");
                else
                    Console.WriteLine("Ошибка при добавлении произвольного элемента");
            }


            Console.WriteLine("======================================");
            Console.WriteLine("Обновим элемент");
            if (Sitemap.UpdateItem(path, url, DateTime.Now, Sitemap.ChangeFrequency.Always, 0.8))
                Console.WriteLine("Произвольный элемент обновлен");
            else
                Console.WriteLine("Ошибка при обновлении произвольного элемента");

            Console.WriteLine("======================================");
            Console.WriteLine("Обновим элемент");
            if (Sitemap.UpdateItem(path, url2, DateTime.Now, Sitemap.ChangeFrequency.Always, 0.8))
                Console.WriteLine("Произвольный элемент обновлен");
            else
                Console.WriteLine("Ошибка при обновлении произвольного элемента");


            Console.WriteLine("======================================");
            Console.WriteLine("Удалим элемент");
            if (Sitemap.RemoveItem(path, url))
                Console.WriteLine("Произвольный элемент удален");
            else
                Console.WriteLine("Ошибка при удалении произвольного элемента");

            Console.Read();
        }
    }
}
