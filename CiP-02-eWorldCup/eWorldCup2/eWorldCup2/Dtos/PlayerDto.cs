namespace eWorldCup2.Api.Dtos
{
    public class PlayerDto
    {
        public string Name { get; set; } = string.Empty;

        //DTO är alltså en rensad och säker version av dina data som du skickar ut från API:et.
        // Den innehåller bara de fält som är nödvändiga för klienten och exponerar inte interna detaljer.

    }
}
