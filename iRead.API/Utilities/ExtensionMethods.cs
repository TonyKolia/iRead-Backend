using iRead.API.Models;

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

        public static IEnumerable<BookResponse> OrderFoundBooks(this List<BookResponse> books, IEnumerable<string> searchItems)
        {
            if (searchItems.Count() == 0 || books.Count() == 0)
                return books;

            var booksRanks = new Dictionary<BookResponse, int>();


            foreach (var book in books)
            {
                var score = 0;
                foreach (var item in searchItems)
                {
                    if (book.Title.Contains(item))
                        score++;
                }

                booksRanks.Add(book, score);
            }

            return booksRanks.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
        }
    }
}
