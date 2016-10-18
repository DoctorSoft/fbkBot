using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Libs;

namespace FacebookBot
{
    public class Account
    {
        public string Login { get; set; }
        public string Pass { get; set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public string LoginProxy { get; set; }
        public string PassProxy { get; set; }
        public string Friends { get; set; }
        public string Requests { get; set; }
        public string NewMessages { get; set; }
        public string Status { get; set; }
        public DateTime DateDisabled { get; set; }
        public double Hours { get; set; }
        public int TimerGroup { get; set; }
        public DateTime TimerGroupDate { get; set; }

        public int Hour { get; set; }
        public int Day { get; set; }
        public int LimitConfirm { get; set; }
        public int LimitConfirmDay { get; set; }
        public int CurrentLimitConfirm { get; set; }
        public int CurrentLimitConfirmDay { get; set; }
        public int LimitGroup { get; set; }
        public int LimitGroupDay { get; set; }
        public int CurrentLimitGroup { get; set; }
        public int CurrentLimitGroupDay { get; set; }
        public int LimitCommunity { get; set; }
        public int LimitCommunityDay { get; set; }
        public int CurrentLimitCommunity { get; set; }
        public int CurrentLimitCommunityDay { get; set; }
        public int LimitDialog { get; set; }
        public int LimitDialogDay { get; set; }
        public int CurrentLimitDialog { get; set; }
        public int CurrentLimitDialogDay { get; set; }
        public int LimitFriends { get; set; }

        public bool Disabled { get; set; }
        public bool Passive { get; set; }
        public bool IsRequest { get; set; }
        public bool IsGroup { get; set; }
        public bool IsMessage { get; set; }
        public bool IsLike { get; set; }
        public bool IsComment { get; set; }
        public bool IsWinking { get; set; }
        public bool IsRemove { get; set; }

        Random random = new Random((int)DateTime.Now.Ticks);

        public void Disable(int h1, int h2)
        {
            var ms1 = Convert.ToInt64(TimeSpan.FromHours(h1).TotalMilliseconds);
            var ms2 = Convert.ToInt64(TimeSpan.FromHours(h2).TotalMilliseconds);
            Hours = Helper.LongRandom(ms1, ms2, random) / 36000d;
            Disabled = true;
            DateDisabled = DateTime.Now;
        }
    }

    public class Friend
    {
        public string AccountLogin { get; set; }
        public string FriendId { get; set; }
        public string FriendName { get; set; }
        public DateTime Date { get; set; }
        public List<Message> Messages { get; set; }
        public double Secs { get; set; }
        public double Mins { get; set; }
        public int Step { get; set; }
        public DateTime ReqDate { get; set; }
        public bool HasNewMessage { get { return Messages.Count > 0 && Messages.Last().Kind == MessageKind.Friend; } }
        public bool Completed { get; set; }
        public bool Added { get; set; }
        public bool AddedGroup { get; set; }
        public bool Liked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateStartedDialog { get; set; }
        public DateTime DateEndedDialog { get; set; }
        public DateTime DateStartedGroups { get; set; }
        public DateTime DateEndedGroups { get; set; }
        public DateTime DateRemoved { get; set; }
        public double PassiveGroupMins { get; set; }
        public double PassiveAddMins { get; set; }

        Random random = new Random((int)DateTime.Now.Ticks);

        public Friend()
        {
            Messages = new List<Message>();
        }

        public void ChangeDate(RandomMessage randomMessages, int groupBefore1, int groupBefore2, int remove1, int remove2, int passiveGroup1, int passiveGroup2, int passiveAdd1, int passiveAdd2, bool refreshDate = true)
        {
            try
            {
                if (refreshDate) Date = DateTime.Now;
                int pause1 = 0, pause2 = 0, pause3 = 0, pause4 = 0;
                if (Step == 7) Completed = true;
                if (Added && AddedGroup)
                {
                    pause1 = remove1;
                    pause2 = remove2;
                    pause3 = remove1 * 60;
                    pause4 = remove2 * 60;
                }
                else if (Step == 7)
                {
                    pause1 = groupBefore1;
                    pause2 = groupBefore2;
                    pause3 = groupBefore1 * 60;
                    pause4 = groupBefore2 * 60;
                    DateEndedDialog = DateTime.Now;
                }
                else
                {
                    pause1 = randomMessages.AnswerPause1[Step];
                    pause2 = randomMessages.AnswerPause2[Step];
                    pause3 = randomMessages.AnswerNoPause1[Step];
                    pause4 = randomMessages.AnswerNoPause2[Step];
                }
                var ms1 = Convert.ToInt64(TimeSpan.FromSeconds(pause1).TotalMilliseconds);
                var ms2 = Convert.ToInt64(TimeSpan.FromSeconds(pause2).TotalMilliseconds);
                Secs = Helper.LongRandom(ms1, ms2, random) / 1000d;
                ms1 = Convert.ToInt64(TimeSpan.FromMinutes(pause3).TotalMilliseconds);
                ms2 = Convert.ToInt64(TimeSpan.FromMinutes(pause4).TotalMilliseconds);
                Mins = Helper.LongRandom(ms1, ms2, random) / 60000d;

                ms1 = Convert.ToInt64(TimeSpan.FromMinutes(passiveGroup1).TotalMilliseconds);
                ms2 = Convert.ToInt64(TimeSpan.FromMinutes(passiveGroup2).TotalMilliseconds);
                PassiveGroupMins = Helper.LongRandom(ms1, ms2, random) / 60000d;
                ms1 = Convert.ToInt64(TimeSpan.FromMinutes(passiveAdd1).TotalMilliseconds);
                ms2 = Convert.ToInt64(TimeSpan.FromMinutes(passiveAdd2).TotalMilliseconds);
                PassiveAddMins = Helper.LongRandom(ms1, ms2, random) / 60000d;
            }
            catch { }
        }

        public void CheckDate(RandomMessage randomMessages, int groupBefore1, int groupBefore2, int remove1, int remove2, int passiveGroup1, int passiveGroup2, int passiveAdd1, int passiveAdd2)
        {
            try
            {
                if (Secs < randomMessages.AnswerPause1[Step] || Secs > randomMessages.AnswerPause2[Step] ||
                    Mins < randomMessages.AnswerNoPause1[Step] || Mins > randomMessages.AnswerNoPause2[Step])
                    ChangeDate(randomMessages, groupBefore1, groupBefore2, remove1, remove2, passiveGroup1, passiveGroup2, passiveAdd1, passiveAdd2, false);
            }
            catch { }
        }
    }

    public class Message
    {
        public MessageKind Kind { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string OfflineThreadingId { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Kind, Text);
        }
    }

    public enum MessageKind
    {
        Bot,
        Friend
    }

    public class RandomMessage
    {
        public int[] AnswerPause1 { get; set; }
        public int[] AnswerPause2 { get; set; }
        public int[] AnswerNoPause1 { get; set; }
        public int[] AnswerNoPause2 { get; set; }
        public int[] AnswerPassivePause1 { get; set; }
        public int[] AnswerPassivePause2 { get; set; }
        public string[][] Answers { get; set; }
    }
}
