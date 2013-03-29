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
        private readonly PlayerInformation playerInformation;
        private SpriteFont font;
        private Texture2D button;

        #endregion

        #region Constructor

        public TemplateManager(ContentManager content,PlayerInformation playerInformation)
        {
            templates = new List<ScoreTemplate>();
            template = content.Load<Texture2D>(@"Textures\Template");
            font = content.Load<SpriteFont>(@"Fonts\pericles10");
            button = content.Load<Texture2D>(@"Textures\smallButton");
            this.playerInformation = playerInformation;
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


        public void addTemplate(string ID, int pos, string sender, GraphicsDevice graphicsDevice)
        {
            Avatar avatar = new Avatar(ID, graphicsDevice);
            templates.Add(new ScoreTemplate(font,button,template, avatar.UsrName,ID, getNewTemplatePos(),avatar.Texture));

            for(int i=0;i<templates.Count;i++)
                OnAddTemplate(templates[i].RealID, i, sender);
        }

        public void addNewTemplate(string ID, int pos, string sender, GraphicsDevice graphicsDevice)
        {
            if (templates.Count <= pos)
            {
                Avatar avatar = new Avatar(ID, graphicsDevice);
                templates.Add(new ScoreTemplate(font, button, template, avatar.UsrName,ID, pos, avatar.Texture));
                if (ID == playerInformation.playerTurn)
                {
                    TileGrid.PlayerID = playerInformation.playerTurn = avatar.UsrName;
                }
            }
             
        }

        public void moveTemplates(Vector2 step)
        {
            foreach (ScoreTemplate template in templates)
            {
                template.Move(step);
            }       
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

        public string getServerName()
        {
            return templates[0].ID;
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
