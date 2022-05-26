namespace Pausable
{
    public interface IPausable
    {
        public bool IsPaused { get; set; }
        public void Pause(bool pause);
    }
    
}