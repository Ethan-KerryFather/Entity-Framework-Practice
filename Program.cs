using Microsoft.EntityFrameworkCore;
using static System.Console;

// 로깅에 필요한 네임스페이스
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
                //// 사용자 지정 콘솔 로거 추가
                //ILoggerFactory loggerFactory =context.GetService<ILoggerFactory>();
                //loggerFactory.AddProvider(new ConsoleLoggerProvider());


                WriteLine($"Products that cost more than a price, highest at top");

                string? input = null;
               // decimal price = 0M;
                string? words = null;

                //do {
                //    Write("Enter a product Price: ");
                //    input = ReadLine();
                //}while(!decimal.TryParse(input, out price));

                Write("Enter words : ");
                words = ReadLine();

                //IQueryable<Product> products = context.Products.Where(product => product.Cost > price)
                //                        .OrderByDescending(product => product.Cost)
                //                        .Select(x => new Product()
                //                        {
                //                            CategoryId = x.ProductId,
                //                            ProductName = x.ProductName,
                //                            Cost = x.Cost,
                //                            ProductId = x.ProductId,    
                //                        });


                var testLikeResults = context.Products
                    .Where(product => EF.Functions
                            .Like(product.ProductName, $"{words}%"))
                    .OrderBy(product => product.ProductId)
                    .Select(product => new {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        cost = product.Cost
                    });

                WriteLine("이름이 포함된 제품 정보\n");
                foreach(var element in testLikeResults)
                {
                    WriteLine(
                        $"\n------------\n제품 아이디 {element.ProductId}\n" +
                        $"제품 이름 {element.ProductName}\t" +
                        $"가격 {element.cost}");
                }


                // 생성된 쿼리문 확인
                WriteLine($"SQL 쿼리문 :{testLikeResults.ToQueryString()}\n\n");
            }
        }
    }
}
