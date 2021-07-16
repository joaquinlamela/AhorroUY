namespace Domain
{
    public class Token
    {
        public string TokenValue { get; set; }
        public Token() { }

        public override bool Equals(object obj)
        {
            return obj is Token token &&
                   TokenValue == token.TokenValue;
        }
    }
}
