#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using System;
using Loki.Generic;
using Loki.Interfaces;
using System.IO;
using System.Collections.Generic;
using Loki.Exceptions;
using Loki.DataRepresentation;
using WebUtilities;
using Loki.DataRepresentation.Loaders;

namespace Odin.Plugin {

    public class LiveChat : WebPluginBase {

        #region Fields

        private Entity intEntity = IntrinsicTypes.Create("System.Int32");
        private Entity stringEntity = IntrinsicTypes.Create("System.String");
        private Entity dateTimeEntity = IntrinsicTypes.Create("System.DateTime");

        #endregion Fields

        #region Utilities

        private void BuildChannel(EntityClass channel)
        {
            EntityField channelId = new EntityField("id");
            channelId.IsRequired = true;
            channelId.IsPrimaryKey = true;
            channelId.IsPreview = true;
            channelId.Type = intEntity;
            channel.Fields.Add(channelId);

            EntityField channelName = new EntityField("name");
            channelName.IsPreview = true;
            channelName.IsRequired = true;
            channelName.Represents = true;
            channelName.Type = stringEntity;
            channel.Fields.Add(channelName);
        }

        private void BuildParticipants(EntityClass participants)
        {
            EntityField participantsId = new EntityField("id");
            participantsId.IsPreview = true;
            participantsId.IsRequired = true;
            participantsId.IsPrimaryKey = true;
            participantsId.Represents = true;
            participantsId.Type = intEntity;
            participants.Fields.Add(participantsId);

            EntityClass principal = (EntityClass)Project.Model.GetByName("Principal");

            EntityField principalRef = new EntityField("principal");
            principalRef.Mult = Multiplicity.ManyToOne;
            principalRef.Type = principal;
            participants.Fields.Add(principalRef);

            EntityField participantRef = new EntityField("Participants");
            participantRef.Mult = Multiplicity.OneToMany;
            participantRef.Type = participants;
            principal.Fields.Add(participantRef);
        }

        private void BuildEntry(EntityClass entry)
        {
            EntityField id = new EntityField("id");
            id.IsPreview = true;
            id.IsRequired = true;
            id.Represents = true;
            id.IsPrimaryKey = true;
            id.Type = intEntity;
            entry.Fields.Add(id);

            EntityField text = new EntityField("text");
            text.IsPreview = false;
            text.Represents = false;
            text.Type = stringEntity;
            text.MaxSize = 3000;
            entry.Fields.Add(text);

            EntityField date = new EntityField("date");
            date.IsPreview = true;
            date.Type = dateTimeEntity;
            entry.Fields.Add(date);

            EntityClass principal = (EntityClass)Project.Model.GetByName("Principal");

            EntityField principalField = new EntityField("Principal");
            principalField.IsPreview = false;
            principalField.Represents = true;
            principalField.Type = principal;
            principalField.Mult = Multiplicity.ManyToOne;
            entry.Fields.Add(principalField);

            EntityField principalEntries = new EntityField("ChannelEntries");
            principalEntries.Type = entry;
            principalEntries.Mult = Multiplicity.OneToMany;
            principal.Fields.Add(principalEntries);
        }

        private void BuildChannelParticipantConnection(EntityClass channel, EntityClass participants)
        {
            EntityField channelParticipants = new EntityField("participants");
            channelParticipants.Type = participants;
            channelParticipants.Mult = Multiplicity.OneToMany;
            channel.Fields.Add(channelParticipants);

            EntityField participantChannel = new EntityField("Channel");
            participantChannel.Type = channel;
            participantChannel.Mult = Multiplicity.ManyToOne;
            participants.Fields.Add(participantChannel);
        }

        private void BuildChannelEntryConnection(EntityClass channel, EntityClass entry)
        {
            EntityField channelEntries = new EntityField("entries");
            channelEntries.Type = channel;
            channelEntries.Mult = Multiplicity.OneToMany;
            channel.Fields.Add(channelEntries);

            EntityField entryChannel = new EntityField("Channel");
            entryChannel.Type = channel;
            entryChannel.Mult = Multiplicity.ManyToOne;
            entry.Fields.Add(entryChannel);
        }

        #endregion Utilities

        #region ICodeGenerator Members

        public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) 
        {
			base.Init( project, dependencies, aggregator );

            EntityClass channel = new EntityClass("Channel", "public");
            EntityClass participants = new EntityClass("ChannelParticipant", "public");
            EntityClass entry = new EntityClass("ChannelEntry", "public");

            BuildChannel(channel);
            BuildParticipants(participants);
            BuildEntry(entry);

            BuildChannelParticipantConnection(channel, participants);
            BuildChannelEntryConnection(channel, entry);

            Project.Model.Add(channel);
            Project.Model.Add(participants);
            Project.Model.Add(entry);

            Directory.CreateDirectory(GetControlsOutputDir("Generic/LiveChat/"));
            GetRelativeOutputDir("",GetControlsOutputDir("WebControls/"));
            GetRelativeOutputDir("",GetControlsOutputDir("WebServices"));
            GetRelativeOutputDir("",GetControlsOutputDir("WebServices/LiveChat/"));
		}

		public override void Generate() {
            GenerateChannelControl();
            GenerateChannelSyncPost();
            GenerateChannelASyncPost();
            GenerateWebService();
            GenerateUserControl();
		}

        private void GenerateUserControl()
        {
            string output = this.GetRelativeOutputDir("Chat.ascx", "UserControls/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Content);

            Dictionary<string, object> variables = BuildArgs();

            Templates.Generate(GetResource("Chat.ascx.vtl"), output, variables);
        }

        private void GenerateWebService()
        {
            Dictionary<string, object> variables = BuildArgs();

            string output = GetRelativeOutputDir("Chat.asmx", "WebServices/LiveChat/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Content);
            Templates.Generate(GetResource("Chat.asmx.vtl"), output, variables);

            string codebehind = GetRelativeOutputDir("Chat.asmx.cs", "WebServices/LiveChat/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), codebehind, true, "Chat.asmx", VSFileType.Compile);
            Templates.Generate(GetResource("Chat.asmx.cs.vtl"), codebehind, variables);
        }

        private void GenerateChannelASyncPost()
        {
            string output = this.GetControlsOutputDir("Generic/LiveChat/ChannelASyncPost.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = BuildArgs();

            Templates.Generate(GetResource("ChannelASyncPost.cs.vtl"), output, variables);
        }

        private void GenerateChannelControl()
        {
            string output = this.GetControlsOutputDir("Generic/LiveChat/ChannelControl.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = BuildArgs();
            Templates.Generate(GetResource("ChannelControl.cs.vtl"), output, variables);
        }

        private Dictionary<string, object> BuildArgs()
        {
            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
            variables.Add("namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString());
            variables.Add("coreMamespace", Project.Name + "." + ComponentType.Core.ToString());
            variables.Add("dalMamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("prefix", Project.Name );
			variables.Add("controls", Assembly + ".Controls");
            return variables;
        }

        private void GenerateChannelSyncPost()
        {
            string output = this.GetControlsOutputDir("Generic/LiveChat/ChannelSyncPost.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = BuildArgs();

            Templates.Generate(GetResource("ChannelSyncPost.cs.vtl"), output, variables);
        }

		public override string Name {
			get { return "Web.Controls.LiveChat"; }
		}

		#endregion

	};

}

