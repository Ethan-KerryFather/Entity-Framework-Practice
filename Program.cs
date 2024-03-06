using Microsoft.EntityFrameworkCore;
using static System.Console;

namespace BookPracEFcore
{
    internal class Program
    {
        static void Main(string[] args)
        {


            TestQueries2();


        }

        public void TestQueries1() {
            Northwind context = new Northwind();

            // lambda

            var lambdaResult = context.Categories.Include(p => p.Products.Where(p => p.Stock >= 10))
                .Select(x => x.CategoryName);

            var lambdaResult2 = context.Products.Include(c => c.Category).Where(x => x.Stock >= 10)
                .Select(x => new
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Stock,
                    Category = x.Category.CategoryName
                });


            // linq

            var linqResult = from element in context.Products
                             orderby element.ProductId ascending
                             select (new
                             {
                                 ProductId = element.ProductId,
                                 productName = element.ProductName,
                             });

            foreach (var result in lambdaResult2)
            {
                Console.WriteLine(result);
            }
        }


        public static void TestQueries2()
        {
            using(Northwind context = new Northwind())
            {
                WriteLine($"Products that cost more than a price, highest at top");

                string? input = null;
                decimal price = 0M;

                do {
                    Write("Enter a product Price: ");
                    input = ReadLine();
                }while(!decimal.TryParse(input, out price));

                IQueryable<Product> products = context.Products.Where(product => product.Cost > price)
                                        .OrderByDescending(product => product.Cost)
                                        .Select(x => new Product()
                                        {
                                            CategoryId = x.ProductId,
                                            ProductName = x.ProductName,
                                            Cost = x.Cost,
                                            ProductId = x.ProductId,    
                                        });

                // 생성된 쿼리문 확인
                WriteLine($"SQL 쿼리문 :{products.ToQueryString()}\n\n");



                foreach(var element in products)
                {
                    WriteLine($"{element.ProductName} , {element.Cost}");
                }

            }
        }
    }
}
