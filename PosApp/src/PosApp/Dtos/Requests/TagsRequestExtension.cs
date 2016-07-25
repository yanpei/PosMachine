using System;
using System.Collections.Generic;
using System.Linq;
using PosApp.Domain;

namespace PosApp.Dtos.Requests
{
    static class TagsRequestExtension
    {
        public static BoughtProduct[] ToBoughtProducts(this IEnumerable<string> tags)
        {
            if (tags == null)
            {
                throw new ArgumentNullException(nameof(tags));
            }

            string[] tagArray = tags.ToArray();
            if (tagArray.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Barcode cannot contains empty one.");
            }

            return tagArray.Select(ToBoughtProduct).ToArray();
        }

        static BoughtProduct ToBoughtProduct(string tag)
        {
            int possibleAmountDividerIndex = tag.LastIndexOf('-');
            if (possibleAmountDividerIndex == -1 || possibleAmountDividerIndex == tag.Length - 1)
            {
                return new BoughtProduct(tag, 1);
            }

            if (possibleAmountDividerIndex == 0)
            {
                int shouldNotBeAmount = TryGetAmount(tag, 0);
                if (shouldNotBeAmount != -1)
                {
                    throw new FormatException($"Invalid tag {tag}");
                }

                return new BoughtProduct(tag, 1);
            }

            int amount = TryGetAmount(tag, possibleAmountDividerIndex);
            return amount == -1
                ? new BoughtProduct(tag, 1)
                : new BoughtProduct(tag.Substring(0, possibleAmountDividerIndex), amount);
        }

        static int TryGetAmount(string tag, int dividerIndex)
        {
            var numberPart = tag.Substring(dividerIndex + 1, tag.Length - dividerIndex - 1);
            int amount;
            if (int.TryParse(numberPart, out amount))
            {
                return amount;
            }

            return -1;
        }
    }
}