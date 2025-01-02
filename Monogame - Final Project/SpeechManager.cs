using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Monogame___Final_Project
{
    internal class SpeechManager
    {
        private List<string> _speechQueue = new List<string>();
        private int _speechIndex = 0;
        private bool _speechIsVisible = false;
        private bool _speechIsDone = false;
        private Vector2 _speechPosition;
        private enum Screen;
        private Screen _screen;

        public SpeechManager(Vector2 initialPosition)
        {
            _speechPosition = initialPosition;
        }

        public void StartSpeech(List<string> speeches)
        {
            _speechQueue = speeches;
            _speechIndex = 0;
            _speechIsDone = false;
            _speechIsVisible = true;
        }
        public void ResetSpeech()
        {
            _speechQueue.Clear();
            _speechIndex = 0;
            _speechIsVisible = false;
            _speechIsDone = false;
        }
        public void EndSpeech()
        {
            _speechIsVisible = false;
            _speechIsDone = true;
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            if (_speechIsVisible && keyboardState.IsKeyDown(Keys.Enter) && prevKeyboardState.IsKeyUp(Keys.Enter))
            {
                _speechIndex++;
                if (_speechIndex >= _speechQueue.Count)
                {
                    _speechIsVisible = false;
                    _speechIsDone = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Texture2D speechTexture)
        {
            if (_speechIsVisible && _speechIndex < _speechQueue.Count)
            {
                string currentSpeech = _speechQueue[_speechIndex];
                spriteBatch.Draw(speechTexture, new Vector2(_speechPosition.X - 160, _speechPosition.Y - 7), Color.White);
                spriteBatch.DrawString(font, currentSpeech, _speechPosition, Color.Black);
            }
        }

        public bool IsSpeechDone
        {
            get { return _speechIsDone; }
        }
        public bool IsSpeechVisible
        {
            get { return _speechIsVisible; }
        }
        public Vector2 SpeechPosition
        {
            get { return _speechPosition; }
            set { _speechPosition = value; }
        }
    }
}
