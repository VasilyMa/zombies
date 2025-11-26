namespace Client 
{
    struct AnimationSwitchStateEvent 
    {
        public AnimationState Animation;
        public enum AnimationState { idle, run, combat }
    }
}
