//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoToiletPaper2023.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class MaterialsAndProducts
    {
        public int Id { get; set; }
        public int Id_Prod { get; set; }
        public int Id_Material { get; set; }
        public Nullable<int> RequiredAmountMaterials1 { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual Product Product { get; set; }
    }
}