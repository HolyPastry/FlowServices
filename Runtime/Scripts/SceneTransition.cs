using System;


namespace Holypastry.Bakery.Flow
{

    [Serializable]
    public struct SceneTransition
    {
        public string Text;
        public float TextDuration;
        public float FadeDuration;

        public SceneTransition(float fadeDuration)
        {
            this.Text = "";
            this.TextDuration = 0f;
            this.FadeDuration = fadeDuration;
        }
    }
}



