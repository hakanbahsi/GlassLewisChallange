namespace GlassLewisChallange.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Exchange { get; private set; }
        public string Ticker { get; private set; }
        public string Isin { get; private set; }
        public string? Website { get; private set; }
        private Company() { }
        public Company(string name, string exchange, string ticker, string isin, string? website)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.");
            }


            if (string.IsNullOrWhiteSpace(isin) || isin.Length < 2 || !char.IsLetter(isin[0]) || !char.IsLetter(isin[1]))
            {
                throw new ArgumentException("ISIN must start with two letters.");
            }

            Id = Guid.NewGuid();
            Name = name;
            Exchange = exchange;
            Ticker = ticker;
            Isin = isin;
            Website = website;
        }

        public void Update(string name, string exchange, string ticker, string isin, string? website)
        {
            Name = name;
            Exchange = exchange;
            Ticker = ticker;
            Isin = isin;
            Website = website;
        }
    }

}
