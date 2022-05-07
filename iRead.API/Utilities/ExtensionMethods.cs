using iRead.API.Models;

namespace iRead.API.Utilities
{
    public static class ExtensionMethods
    {
        public static object MapResponse<T>(this IEnumerable<T> list)
        {
            //var mappedList = new List<T>();
            //foreach(var input in list)
            //{
            //    mappedList.Add((T)MapResponse<T>(input));
            //}

            var mappedList = new List<object>();
            foreach(var input in list)
            {
                mappedList.Add(MapResponse(input));
            }

            return mappedList;

            //var mapper = new ResponseModelMapper();
            //var type = typeof(T);

            //if (type == typeof(Publisher))
            //    return mapper.MapPublishers((IEnumerable<Publisher>)list);

            //return list;
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


            //var mapper = new ResponseModelMapper();
            //var type = typeof(T);
            //if (type == typeof(Publisher))
            //    return mapper.MapPublisher(input as Publisher);
            //else if (type == typeof(MemberPersonalInfo))
            //    return mapper.MapMemberPersonalInfo(input as MemberPersonalInfo);
            //else if (type == typeof(MemberContactInfo))
            //    return mapper.MapMemberContactInfo(input as MemberContactInfo);

            //return input;
        }
    }
}
