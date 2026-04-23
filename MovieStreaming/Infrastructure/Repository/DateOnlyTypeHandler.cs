using Dapper;
using System.Data;

namespace MovieStreaming.Infrastructure.Repository
{
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value)
        {
            return DateOnly.FromDateTime((DateTime)value);
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToDateTime(TimeOnly.MinValue);
        
        }
    }
}
