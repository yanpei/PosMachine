﻿using System.Linq;
using System.Text;
using NHibernate.Util;
using PosApp.Domain;

namespace PosApp.Dtos.Responses
{
    static class ReceiptDtoExtensions
    {
        public static string ToReceiptDto(this Receipt receipt)
        {
            StringBuilder receiptBuilder = new StringBuilder(256)
                .AppendLine("Receipt:")
                .AppendLine("--------------------------------------------------");

            receipt.ReceiptItems.OrderBy(ri => ri.Product.Name)
                .Select(ri =>
                {
                    string price = ri.Total.ToString("F2");
                    return $"Product: {ri.Product.Name}, Amount: {ri.Amount}, Price: {price}, Promoted: {ri.Promoted.ToString("F2")}";
                })
                .ForEach(ri => receiptBuilder.AppendLine(ri));

            return receiptBuilder
                .AppendLine("--------------------------------------------------")
                .Append($"Promoted: {receipt.Promoted.ToString("F2")}")
                .Append($"Total: {receipt.Total.ToString("F2")}")
                .ToString();
        }
    }
}