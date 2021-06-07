namespace ch9_security_in_practice
{
    public class SecretPackage
    {
        public byte[] IV { get; set; }
        public byte[] SessionKey { get; set; }
        public byte[] Message { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Signature { get; set; }
    }
}
