using System;
using System.IO;
using System.Linq;
using FicaEmCasaFunctionApp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;

namespace FicaEmCasaFunctionApp.Functions
{
    public static class ProdutosFunction
    {
        [FunctionName(nameof(ProdutosFunction) + "_" + nameof(Get))]
        public static IActionResult Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "produtos")]
            HttpRequest httpRequest)
        {
            return new OkObjectResult(Produto.Itens);
        }

        [FunctionName(nameof(ProdutosFunction) + "_" + nameof(GetById))]
        public static IActionResult GetById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "produtos/{produtoId}")]
            HttpRequest httpRequest,
            Guid produtoId)
        {
            var produto = Produto.Itens.FirstOrDefault(f => f.Id == produtoId);

            if (produto == null)
                return new NotFoundResult();

            return new OkObjectResult(produto);
        }

        [FunctionName(nameof(ProdutosFunction) + "_" + nameof(GetByIdFromQueryString))]
        public static IActionResult GetByIdFromQueryString(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "produto")]
            HttpRequest httpRequest)
        {
            var produtoId = new Guid(httpRequest.Query["produtoId"]);
            var produto = Produto.Itens.FirstOrDefault(f => f.Id == produtoId);

            if (produto == null)
                return new NotFoundResult();

            return new OkObjectResult(produto);
        }

        [FunctionName(nameof(ProdutosFunction) + "_" + nameof(Post))]
        public static IActionResult Post(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "produtos")]
            HttpRequest httpRequest)
        {
            using var streamReader = new StreamReader(httpRequest.Body);
            var json = streamReader.ReadToEnd();
            var produto = JsonConvert.DeserializeObject<Produto>(json);

            Produto.Itens.Add(produto);

            return new OkResult();
        }

        //0 */5 * * * *	every 5 minutes
        //0 0 */6 * * *	every 6 hours
        [FunctionName(nameof(ProdutosFunction) + "_" + nameof(AtualizarEstoque))]
        public static void AtualizarEstoque([TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo)
        {
            var random = new Random();

            foreach (var produto in Produto.Itens)
            {
                var quantidade = random.Next(-200, 200);
                produto.AtualizarEstoque(quantidade);
            }
        }
    }
}