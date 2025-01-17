﻿using System.Collections.Generic;

namespace OngekiFumenEditor.Base.OngekiObjects
{
    //[DontShowPropertyInfoAttrbute]
    public class Bullet : OngekiMovableObjectBase, IBulletPalleteReferencable
    {
        private BulletPallete referenceBulletPallete;
        public BulletPallete ReferenceBulletPallete
        {
            get { return referenceBulletPallete; }
            set
            {
                referenceBulletPallete = value;
                NotifyOfPropertyChange(() => ReferenceBulletPallete);
            }
        }

        public enum BulletDamageType
        {
            Normal = 0,
            Hard = 1,
            Danger = 2
        }

        /*
        public class BulletDamageType : FadeStringEnum
        {
            public BulletDamageType(string value) : base(value)
            {

            }

            /// <summary>
            /// 将使用BULLET_DAMAGE伤害
            /// </summary>
            public static BulletDamageType Normal { get; } = new BulletDamageType("NML");
            /// <summary>
            /// 将使用HARDBULLET_DAMAGE伤害
            /// </summary>
            public static BulletDamageType Hard { get; } = new BulletDamageType("STR");
            /// <summary>
            /// 将使用DANGERBULLET_DAMAGE伤害
            /// </summary>
            public static BulletDamageType Danger { get; } = new BulletDamageType("DNG");
        }
        */

        public Bullet()
        {

        }

        public override IEnumerable<IDisplayableObject> GetDisplayableObjects()
        {
            yield return this;
        }

        private BulletDamageType bulletDamageTypeValue = BulletDamageType.Normal;
        public BulletDamageType BulletDamageTypeValue
        {
            get { return bulletDamageTypeValue; }
            set
            {
                bulletDamageTypeValue = value;
                NotifyOfPropertyChange(() => BulletDamageTypeValue);
            }
        }


        public override string IDShortName => CommandName;

        public const string CommandName = "BLT";

        public override string ToString() => $"{base.ToString()} Pallete[{ReferenceBulletPallete}] DamageType[{BulletDamageTypeValue}]";

        public override void Copy(OngekiObjectBase fromObj)
        {
            base.Copy(fromObj);

            if (fromObj is not Bullet from)
                return;

            ReferenceBulletPallete = from.ReferenceBulletPallete;
            BulletDamageTypeValue = from.BulletDamageTypeValue;
        }
    }
}
