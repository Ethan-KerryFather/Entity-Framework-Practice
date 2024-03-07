using Microsoft.EntityFrameworkCore;
using static System.Console;

// 로깅에 필요한 네임스페이스
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//
using Microsoft.EntityFrameworkCore.ChangeTracking;
using BookPracEFcore.AutoGen;
using Microsoft.EntityFrameworkCore.Update;

namespace BookPracEFcore
{
    public class Program
    {
        static void Main(string[] args)
        {

        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="productNameStartsWith"></param>
        /// <returns></returns>
        public static int TestQueries8(string productNameStartsWith)
        {
            using (var db = new Northwind())
            {
                IQueryable<Product>? products = db.Products
                    .Where(product => product.ProductName.StartsWith(productNameStartsWith));
                if(products is null)
                {
                    WriteLine("No product is available");
                }
                else
                {
                    db?.Products.RemoveRange(products);
                }
                int affected = db.SaveChanges();
                return affected;
            }

        }

        /// <summary>
        /// 내용 수정
        /// </summary>
        /// <param name="productNameStartsWith"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static bool TestQueries7(string productNameStartsWith, decimal amount)
        {
            using (var db = new Northwind())
            {
                int affected = 0;

                WriteLine($"{db.Products.FirstOrDefault(product => product.ProductName.StartsWith(productNameStartsWith))?.ProductName}");
                WriteLine($"{db.Products.FirstOrDefault(product => product.ProductName.StartsWith(productNameStartsWith))?.Cost}");


                var product = db.Products.FirstOrDefault(product => product.ProductName.StartsWith(productNameStartsWith));
                if (product != null)
                    
                {
                    WriteLine(product?.ProductName);
                    product.Cost += amount;
                    WriteLine(product?.Cost);
                    affected = db.SaveChanges();

                }
            }

            return false;
        }

        public static void TestQueries6()
        {
            using (var context = new Northwind())
            {
                var lambdaResult = context.Products
                    .Where( product => product.Discontinued != true)
                    .Include( product => product.Category)
                    .Select(product => new 
                    {
                        Id = product.ProductId,
                        ProductName = product.ProductName,
                        Cost = product.Cost,
                        Stock = product.Stock,
                        Disc = product.Discontinued,
                        CategoryName = product.Category.CategoryName,
                    });
                    
                foreach(var element in lambdaResult)
                {
                    WriteLine($"{element.Id,-3} {element.CategoryName, -3} {element.ProductName, -3}" +
                        $"{element.Cost, 8} {element.Stock, -3} " +
                        $"{element.Disc, -3}");
                }
            }
        }

        /// <summary>
        /// 프로덕트 추가
        /// </summary>
        /// <param name="categoryId">카테고리 아이디</param>
        /// <param name="productName"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        public static bool TestQueries5(int categoryId, string productName, decimal? price)
        {
            using (var context = new Northwind())
            {
                Product product = new Product
                {
                    CategoryId = categoryId,
                    ProductName = productName,
                    Cost = price
                };

                context.Products.Add(product);

                int affected = context.SaveChanges();
                return (affected == 1);
            }
        }


        public static void TestQueries4()
        {
            using(var context = new Northwind())
            {
                // 지연로딩 비활성화 
                context.ChangeTracker.LazyLoadingEnabled = false;

                var category = context.Categories.Find(1);
                //Categories.Find() 안에는 Category의 주키가 들어간다. 



                if (category != null)
                {
                    context.Entry(category)
                        .Collection(category => category.Products).Load();
                    // Category.Products는 Category 모델의 네비게이션 프로퍼티였다. ICollection
                }

                IQueryable<Category> categories = context.Categories.Include(category => category.Products);



                foreach (Category element in categories)
                {
                    WriteLine($"{element.CategoryName} has {element.Products.Count} products");
                }
            }
        }

        public static void TestQueries3()
        {
            var context = new Northwind();
            
            // 지연로딩 비활성화
            //context.ChangeTracker.LazyLoadingEnabled = false;
            IQueryable<Category> categories = context.Categories.Include(category => category.Products);

           

            foreach(Category category in categories)
            {
                WriteLine($"{category.CategoryName} has {category.Products.Count} products");
            }
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
                WriteLine($"\nSQL 쿼리문 :{testLikeResults.ToQueryString()}\n\n");
            }
        }
    
        
    }
}
