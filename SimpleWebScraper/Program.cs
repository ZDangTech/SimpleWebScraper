// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using SimpleWebScraper;
using CsvHelper;
using System.Globalization;

var web = new HtmlWeb();
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

