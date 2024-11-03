using AutoMapper;

namespace WebApp.BusinessLogic.Validation
{
    public static class MapperExtensions
    {
        public static T MapWithExceptionHandling<T>(this IMapper mapper, object source)
        {
            ArgumentNullException.ThrowIfNull(mapper);

            try
            {
                return mapper.Map<T>(source);
            }
            catch (AutoMapperMappingException ex)
            {
                throw new ForumException($"Mapping failed for type {typeof(T).Name}", ex);
            }
        }
    }
}
