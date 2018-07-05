// <copyright file="SlackHealthAlertOptions.cs" company="App Metrics Contributors">
// Copyright (c) App Metrics Contributors. All rights reserved.
// </copyright>

namespace App.Metrics.Health.Alerts.Slack
{
    public class SlackHealthAlertOptions
    {
        public string Channel { get; set; }

        public string WebhookUrl { get; set; }

        public string Username { get; set; }

        public string EmojiIcon { get; set; }

        public bool AlertOnDegradedChecks { get; set; } = true;
    }
}