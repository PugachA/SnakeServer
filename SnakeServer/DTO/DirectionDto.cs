using SnakeServer.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SnakeServer.DTO
{
    /// <summary>
    /// Класс для сериализации POST запроса с изменением направления движения
    /// </summary>
    public class DirectionDto
    {
        [Required(ErrorMessage = "Укажите направление")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Direction Direction { get; set; }
    }
}
