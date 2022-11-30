using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tank.PaymentAPI.Models;
using Tank.PaymentAPI.Services;

namespace Tank.PaymentAPI.Datas
{
    public partial class MyDbTankContext : DbContext
    {
        private readonly TankSetting _tankSetting;

        public MyDbTankContext(DbContextOptions<MyDbTankContext> options, IOptionsMonitor<TankSetting> optionsMonitor) : base(options) 
        {
            _tankSetting = optionsMonitor.CurrentValue;
        }
        public MyDbTankContext()
        {

        }

        #region DbSet
        public virtual DbSet<ChargeMoneyModel> ChargeMoneys { get; set; }
        public virtual DbSet<SysUsersDetailModel> SysUsersDetails { get; set; }
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(string.Format(_tankSetting.Tank_ConnectionString, 
                _tankSetting.Tank_Source, 
                _tankSetting.Tank_Port, 
                _tankSetting.Tank_Catalog, 
                _tankSetting.Tank_UserID, 
                _tankSetting.Tank_Password));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Vietnamese_CI_AS");

            #region ChargeMoney - Nạp xu
            modelBuilder.Entity<ChargeMoneyModel>(entity =>
            {
                entity.HasKey(e => e.ChargeId);

                entity.ToTable("Charge_Money");

                entity.HasComment("充值记录表");

                entity.HasIndex(e => e.UserName, "IX_Charge_Money")
                    .HasFillFactor((byte)90);

                entity.Property(e => e.ChargeId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ChargeID")
                    .HasComment("<充值记录表>支付订单编号");

                entity.Property(e => e.CanUse).HasComment("是否已经充值0表示已到帐 1表示未到帐");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasComment("支付日期");

                entity.Property(e => e.Ip)
                    .HasMaxLength(50)
                    .HasColumnName("IP")
                    .HasComment("充值ip地址");

                entity.Property(e => e.Money).HasComment("点劵");

                entity.Property(e => e.NeedMoney)
                    .HasColumnType("decimal(10, 2)")
                    .HasComment("所需金钱");

                entity.Property(e => e.NickName)
                    .HasMaxLength(200)
                    .HasComment("充值昵称");

                entity.Property(e => e.PayWay)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("((0))")
                    .HasComment("支付方式");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasComment("用户名");
            });
            #endregion

            #region Sys_Users_Detail
            modelBuilder.Entity<SysUsersDetailModel>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK_Sys_Users_Detail_1");

                entity.ToTable("Sys_Users_Detail");

                entity.HasComment("用户资料表");

                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasComment("用户编号");

                entity.Property(e => e.AchievementPoint).HasComment("VIP");

                entity.Property(e => e.ActiveIp)
                    .HasMaxLength(50)
                    .HasColumnName("ActiveIP")
                    .HasComment("激活IP");

                entity.Property(e => e.AddDayGp)
                    .HasColumnName("AddDayGP")
                    .HasComment("日增GP");

                entity.Property(e => e.AddDayOffer).HasComment("日增长功勋");

                entity.Property(e => e.AddWeekGp)
                    .HasColumnName("AddWeekGP")
                    .HasComment("周增长GP");

                entity.Property(e => e.AddWeekOffer).HasComment("周增长功勋");

                entity.Property(e => e.AnswerSite).HasComment("用户答题位置");

                entity.Property(e => e.AntiAddiction).HasComment("防沉迷系统时间");

                entity.Property(e => e.AntiDate)
                    .HasColumnType("datetime")
                    .HasComment("防沉迷时间计算");

                entity.Property(e => e.ApprenticeshipState).HasColumnName("apprenticeshipState");

                entity.Property(e => e.BadLuckNumber).HasColumnName("badLuckNumber");

                entity.Property(e => e.ChargeDate)
                    .HasColumnType("datetime")
                    .HasComment("最近充值时间");

                entity.Property(e => e.ChatCount).HasComment("VIP");

                entity.Property(e => e.CheckCount).HasComment("用户输入验证码次数");

                entity.Property(e => e.Colors)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N',,,,,,')")
                    .HasComment("样式颜色");

                entity.Property(e => e.ConsortiaId)
                    .HasColumnName("ConsortiaID")
                    .HasComment("工会ID");

                entity.Property(e => e.DamageScores).HasColumnName("damageScores");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("激活日期");

                entity.Property(e => e.DayLoginCount).HasComment("日登陆次数");

                entity.Property(e => e.DdplayPoint).HasColumnName("DDPlayPoint");

                entity.Property(e => e.Escape).HasComment("逃跑数");

                entity.Property(e => e.ExpendDate)
                    .HasColumnType("datetime")
                    .HasComment("最近消费时间");

                entity.Property(e => e.FightLabPermission)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('0000000000')")
                    .HasComment("VIP");

                entity.Property(e => e.FightPower).HasComment("战斗力");

                entity.Property(e => e.ForbidDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("禁号时间");

                entity.Property(e => e.ForbidReason)
                    .HasMaxLength(1000)
                    .HasComment("禁号原因");

                entity.Property(e => e.FreezesDate)
                    .HasColumnType("datetime")
                    .HasColumnName("freezesDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GetSoulCount).HasDefaultValueSql("((30))");

                entity.Property(e => e.GiftGp).HasComment("VIP");

                entity.Property(e => e.GiftLevel).HasComment("VIP");

                entity.Property(e => e.GiftToken).HasComment("礼券");

                entity.Property(e => e.Gold).HasComment("金币");

                entity.Property(e => e.Gp)
                    .HasColumnName("GP")
                    .HasComment("GP");

                entity.Property(e => e.Grade)
                    .HasDefaultValueSql("((1))")
                    .HasComment("等级");

                entity.Property(e => e.GraduatesCount).HasColumnName("graduatesCount");

                entity.Property(e => e.HardCurrency).HasColumnName("hardCurrency");

                entity.Property(e => e.Hide)
                    .HasDefaultValueSql("((1111111111))")
                    .HasComment("是否显示");

                entity.Property(e => e.Honor)
                    .HasMaxLength(50)
                    .HasComment("头衔<暂不用到>");

                entity.Property(e => e.HonourOfMaster)
                    .HasMaxLength(500)
                    .HasColumnName("honourOfMaster")
                    .HasDefaultValueSql("(N'')")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.IsConsortia).HasComment("是否加入工会");

                entity.Property(e => e.IsCreatedMarryRoom).HasComment("是否创建结婚房间");

                entity.Property(e => e.IsExist)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否存在");

                entity.Property(e => e.IsFirst).HasComment("共总登陆次数");

                entity.Property(e => e.IsFirstDivorce)
                    .HasColumnName("isFirstDivorce")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsFistGetPet)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsGotRing).HasComment("是否得到戒指");

                entity.Property(e => e.IsInSpaPubGoldToday).HasComment("VIP");

                entity.Property(e => e.IsInSpaPubMoneyToday).HasComment("VIP");

                entity.Property(e => e.IsMarried).HasComment("是否结婚");

                entity.Property(e => e.IsOldPlayer).HasComment("VIP");

                entity.Property(e => e.IsOldPlayerHasValidEquitAtLogin).HasColumnName("isOldPlayerHasValidEquitAtLogin");

                entity.Property(e => e.IsOpenGift).HasComment("VIP");

                entity.Property(e => e.IsViewer).HasColumnName("isViewer");

                entity.Property(e => e.LastAuncherAward)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("登录器最近获奖日期");

                entity.Property(e => e.LastAward)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最近获奖日期");

                entity.Property(e => e.LastDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最后活动日期");

                entity.Property(e => e.LastDateSecond)
                    .HasColumnType("datetime")
                    .HasComment("倒数第二次登陆时间");

                entity.Property(e => e.LastDateThird)
                    .HasColumnType("datetime")
                    .HasComment("倒数第三次登陆时间");

                entity.Property(e => e.LastDayGp)
                    .HasColumnName("LastDayGP")
                    .HasComment("最近一次经验");

                entity.Property(e => e.LastDayOffer).HasComment("最后日功勋");

                entity.Property(e => e.LastGetEgg)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLuckNum).HasColumnName("lastLuckNum");

                entity.Property(e => e.LastLuckyNumDate)
                    .HasColumnType("datetime")
                    .HasColumnName("lastLuckyNumDate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastRefreshPet)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastSpaDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("VIP");

                entity.Property(e => e.LastWeekGp)
                    .HasColumnName("LastWeekGP")
                    .HasComment("最后周GP");

                entity.Property(e => e.LastWeekOffer).HasComment("最后周功勋");

                entity.Property(e => e.LastWeekly)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("VIP");

                entity.Property(e => e.LastWeeklyVersion).HasComment("VIP");

                entity.Property(e => e.LoginCount).HasComment("登陆次数(一日一次)");

                entity.Property(e => e.LuckyNum).HasColumnName("luckyNum");

                entity.Property(e => e.MarryInfoId)
                    .HasColumnName("MarryInfoID")
                    .HasComment("交友中心ID");

                entity.Property(e => e.MasterId).HasColumnName("masterID");

                entity.Property(e => e.MasterOrApprentices)
                    .HasMaxLength(500)
                    .HasColumnName("masterOrApprentices")
                    .HasDefaultValueSql("(N'')")
                    .UseCollation("SQL_Latin1_General_CP1_CI_AS");

                entity.Property(e => e.Medal).HasComment("Medal");

                entity.Property(e => e.Money).HasComment("点卷");

                entity.Property(e => e.MyHonor).HasColumnName("myHonor");

                entity.Property(e => e.MyScore).HasColumnName("myScore");

                entity.Property(e => e.NecklaceExp).HasColumnName("necklaceExp");

                entity.Property(e => e.NecklaceExpAdd).HasColumnName("necklaceExpAdd");

                entity.Property(e => e.NeedGetBoxTime).HasColumnName("needGetBoxTime");

                entity.Property(e => e.NickName)
                    .HasMaxLength(50)
                    .HasComment("用户昵称");

                entity.Property(e => e.Nimbus).HasComment("光环");

                entity.Property(e => e.Offer).HasComment("功勋");

                entity.Property(e => e.OnlineTime).HasComment("在线时长(分钟)");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasComment("密码");

                entity.Property(e => e.PasswordTwo)
                    .HasMaxLength(200)
                    .HasComment("第二层密码");

                entity.Property(e => e.PetScore).HasColumnName("petScore");

                entity.Property(e => e.PvePermission)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('11111111111111111111111111111111111111111111111111')")
                    .HasComment("难度等级");

                entity.Property(e => e.QuestSite)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(0x)");

                entity.Property(e => e.ReceieGrade).HasColumnName("receieGrade");

                entity.Property(e => e.Receiebox).HasColumnName("receiebox");

                entity.Property(e => e.Rename).HasComment("是否重名");

                entity.Property(e => e.Repute).HasComment("GP经验排名");

                entity.Property(e => e.ReputeOffer).HasComment("用户捐献");

                entity.Property(e => e.RichesOffer).HasComment("贡献财富");

                entity.Property(e => e.RichesRob).HasComment("掠夺财富");

                entity.Property(e => e.SelfMarryRoomId)
                    .HasColumnName("SelfMarryRoomID")
                    .HasComment("结婚房间ID");

                entity.Property(e => e.ServerName)
                    .HasMaxLength(50)
                    .HasComment("服务器名");

                entity.Property(e => e.Sex)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("性别");

                entity.Property(e => e.ShareFb).HasColumnName("ShareFB");

                entity.Property(e => e.Site)
                    .HasMaxLength(200)
                    .HasComment("来源站点");

                entity.Property(e => e.Skin)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')")
                    .HasComment("皮肤");

                entity.Property(e => e.SpaPubGoldRoomLimit).HasComment("VIP");

                entity.Property(e => e.SpaPubMoneyRoomLimit).HasComment("VIP");

                entity.Property(e => e.SpouseId)
                    .HasColumnName("SpouseID")
                    .HasComment("配偶ID");

                entity.Property(e => e.SpouseName)
                    .HasMaxLength(50)
                    .HasComment("配偶名称");

                entity.Property(e => e.State).HasComment("用户状态");

                entity.Property(e => e.Style)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("(N',,,,,,')")
                    .HasComment("用户模板");

                entity.Property(e => e.TimeBox)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Total).HasComment("总场数");

                entity.Property(e => e.TotemId).HasColumnName("totemId");

                entity.Property(e => e.UesedFinishTime).HasColumnName("uesedFinishTime");

                entity.Property(e => e.UserName)
                    .HasMaxLength(200)
                    .HasComment("用户昵称");

                entity.Property(e => e.WeaklessGuildProgressStr)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasDefaultValueSql("(N'AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=')")
                    .HasComment("VIP");

                entity.Property(e => e.Win).HasComment("胜场数");
            });
            #endregion

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
