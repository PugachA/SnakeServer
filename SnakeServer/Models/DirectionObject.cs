using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeServer.Models
{
    /// <summary>
    /// Класс для сериализации POST запроса с изменением направления движения
    /// </summary>
    public class DirectionObject
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
    }

    /// <summary>
    /// Перечисление в возможными направлениями движения
    /// </summary>
    public enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }
}
