using iRead.API.Models;
using iRead.API.Models.Recommendation;
using iRead.DBModels.CustomModels;
using iRead.RecommendationSystem.Models;
using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace iRead.API.Utilities
{
    public static class ExtensionMethods
    {
        public static object MapResponse<T>(this IEnumerable<T> list)
        {
            var mappedList = new List<object>();
            foreach(var input in list)
            {
                mappedList.Add(MapResponse(input));
            }

            return mappedList;
        }



        public static object MapResponse<T>(this T input)
        {
            var type = Type.GetType($"iRead.API.Models.{typeof(T).Name}Response");
            if (type == null)
                return input;

            var mappedObject = Activator.CreateInstance(type);
            if (mappedObject == null)
                return input;

            foreach (var prop in mappedObject.GetType().GetProperties())
            {
                var property = input?.GetType().GetProperty(prop.Name);
                if (property != null)
                    prop.SetValue(mappedObject, property.GetValue(input));
            }

            return mappedObject;
        }

        public static IEnumerable<T> CastObjectToList<T>(this object list)
        {
            return (list as IEnumerable<object>)?.Cast<T>().ToList();
        }

        public static IEnumerable<int> ContertToInteger(this string[] array)
        {
            var intList = new List<int>();
            foreach(var str in array)
            {
                intList.Add(int.Parse(str));
            }

            return intList;
        }

        public static List<BookResponse> OrderFoundBooks(this List<BookResponse> books, IEnumerable<string> searchItems)
        {
            if (searchItems.Count() == 0 || books.Count() == 0)
                return books;

            var booksRanks = new Dictionary<BookResponse, int>();

            foreach (var book in books)
            {
                var score = 0;

                foreach (var item in searchItems)
                {
                    if (book.Title.ToLower().RemoveDiacritics().Contains(item.ToLower().RemoveDiacritics()))
                        score++;
                }

                booksRanks.Add(book, score);
            }

            return booksRanks.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        }

        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString.EnumerateRunes())
            {
                var unicodeCategory = Rune.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        //randomly order a list using guid
        public static IEnumerable<T> RandomlyOrderList<T>(this IEnumerable<T> list)
        {
            return list.Select(x => new { Guid = Guid.NewGuid().ToString(), Item = x }).OrderBy(x => x.Guid).Select(x => x.Item);
        }

        //Order the returned books in the main page using the following "ranking" 
        //1. Books recommended from the recommendation engine
        //2. Books written by a favorite author in a favorite category
        //3. Books in a favorite category
        //4. The rest of the books
        public static List<BookResponse> OrderFoundBooksBasedOnRecommendations(this List<BookResponse> books, IEnumerable<RecommendedBook> recommendedBooks, IEnumerable<int> favoriteCategories, IEnumerable<int> favoriteAuthors, bool fromSearch = false)
        {
            if (recommendedBooks.Count() == 0 || books.Count == 0)
                return books;

            //this list contains the 4 above mentions ranking "partitions"
            var booksParts = new List<IEnumerable<BookResponse>>();

            //partition 1. Books recommended from the recommendation engine
            booksParts.Add(books.Where(x => recommendedBooks.Select(r => r.BookId).Contains(x.Id)));

            var booksNotInRecommended = books.Where(x => !recommendedBooks.Select(r => r.BookId).Contains(x.Id));
            var recommendedByCategory = new List<BookResponse>();
            var recommendedByCategoryAndAuthor = new List<BookResponse>();
            foreach (var favCategory in favoriteCategories)
            {
                recommendedByCategory.AddRange(booksNotInRecommended.Where(x => x.Categories.Select(c => c.Id).Contains(favCategory)));
                foreach (var favAuthor in favoriteAuthors)
                {
                    //Partition 2. Books written by a favorite author in a favorite category
                    recommendedByCategoryAndAuthor.AddRange(recommendedByCategory.Where(x => x.Authors.Select(a => a.Id).Contains(favAuthor)));
                }
            }

            //partition 3.Books in a favorite category
            //we remove the books in partition 2 from partition 3 to avoid duplicates
            if (recommendedByCategoryAndAuthor.Count > 0)
                recommendedByCategory.RemoveAll(x => recommendedByCategoryAndAuthor.Contains(x));

            //random ordering for results if not from search
            //if from search, a search string match "sub ordering" must exist inside each part as it is already ordered like this
            if (!fromSearch)
            {
                recommendedByCategoryAndAuthor = recommendedByCategoryAndAuthor.RandomlyOrderList().ToList();
                recommendedByCategory = recommendedByCategory.RandomlyOrderList().ToList();
            }
                
            booksParts.Add(recommendedByCategoryAndAuthor);                
            booksParts.Add(recommendedByCategory);

            //partition 4. The rest of the books
            //remove any books not recommended in any way to avoid duplicates
            var notRecommended = booksNotInRecommended.Where(x => !recommendedByCategory.Contains(x) && !recommendedByCategoryAndAuthor.Contains(x));
            if (!fromSearch)
                notRecommended = notRecommended.RandomlyOrderList();
            booksParts.Add(notRecommended);

            //add all the partitions in a new list in the correct order which will be the displaying order
            var orderedBooks = new List<BookResponse>();
            foreach (var booksPart in booksParts)
                orderedBooks.AddRange(booksPart);

            return orderedBooks;
        }
    
        public static IEnumerable<TrainingInput> TransformToTrainingData(this IEnumerable<RecommenderTrainingData> trainData)
        {
            var list = new List<TrainingInput>();
            foreach(var data in trainData)
            {
                list.Add(new TrainingInput(Convert.ToUInt16(data.UserId), Convert.ToUInt16(data.BookId), (float)data.Rating));
            }
            return list;
        }
    }
}
