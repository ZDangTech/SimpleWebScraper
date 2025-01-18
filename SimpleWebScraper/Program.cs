// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using SimpleWebScraper;
using CsvHelper;
using System.Globalization;
using HtmlAgilityPack.CssSelectors.NetCore;

var web = new HtmlWeb();
// setting a global User-Agent header in HAP 
web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36";
var document = web.Load("https://www.scrapingcourse.com/ecommerce/");
var products = new List<Product>();

// Selecting all HTML product elements from the current page 
var productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product");

// Iterating over the list of product elements to scape data to the class 
foreach (var productHTMLElement in productHTMLElements)
{
    var url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes["href"].Value);
    var image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes["src"].Value);
    var name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText);
    var price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText);
    var product = new Product { Url = url, Image = image, Name = name, Price = price };
    products.Add(product);
}

using(var writer = new StreamWriter("products.csv"))

using(var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(products);
}


// print data in console
foreach (var product in products)
{
    Console.WriteLine($"Name: {product.Name}");
    Console.WriteLine($"Price: {product.Price}");
    Console.WriteLine($"URL: {product.Url}");
    Console.WriteLine($"Image: {product.Image}");
    Console.WriteLine();
}

