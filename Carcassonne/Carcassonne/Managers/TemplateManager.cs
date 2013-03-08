using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TileEngine.Entity;

namespace Carcassonne
{
    using MultiplayerGame.Args;

    public class TemplateManager
    {

        #region Declarations

        public event EventHandler<TemplateArgs> RequestTemplate;
        public event EventHandler<TemplateArgs> AddTemplate;
        private List<ScoreTemplate> templates;
        private Texture2D template;
        private SpriteFont font;
        private Texture2D button;

        #endregion

        #region Constructor

        public TemplateManager(ContentManager content)
        {
            templates = new List<ScoreTemplate>();
            template = content.Load<Texture2D>(@"Textures\Template");
            font = content.Load<SpriteFont>(@"Fonts\pericles10");
            button = content.Load<Texture2D>(@"Textures\smallButton");
        }

        #endregion

        #region Public Methods

        public void OnRequestTemplate(string name,string owner)
        {
            EventHandler<TemplateArgs> requestTemplate = RequestTemplate;
            if (requestTemplate != null)
                requestTemplate(requestTemplate, new TemplateArgs(name,getNewTemplatePos(),owner));
        }

        public void OnAddTemplate(string name,int pos, string owner)
        {
            EventHandler<TemplateArgs> addTemplate = AddTemplate;
            if (addTemplate != null)
                addTemplate(addTemplate, new TemplateArgs(name, pos, owner));
        }


        public void addTemplate(string ID, int pos,string sender)
        {
            templates.Add(new ScoreTemplate(font,button,template, ID, getNewTemplatePos()));
            for(int i=0;i<templates.Count;i++)
                OnAddTemplate(templates[i].ID, i, sender);
        }

        public void addNewTemplate(string ID, int pos, string sender)
        {
            if(templates.Count<=pos)
            templates.Add(new ScoreTemplate(font, button, template, ID, pos));
      
             
        }

        public int getNewTemplatePos()
        {
            return templates.Count();
        }

        public void updateTemplate(string name,int value,string sender)
        {
            foreach (ScoreTemplate template in templates)
                if (template.ID == name)
                    template.score = value;
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {

            foreach (ScoreTemplate template in templates)
                template.Update(gameTime);
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ScoreTemplate template in templates)
                template.Draw(spriteBatch);

        }

        #endregion

    }
}
