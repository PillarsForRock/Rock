//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Rock.Models;

namespace Rock.Models.Cms
{
    [Table( "cmsPage" )]
    public partial class Page : ModelWithAttributes, IAuditable
    {
		[DataMember]
		public Guid Guid { get; set; }
		
		[MaxLength( 100 )]
		[DataMember]
		public string Name { get; set; }
		
		[MaxLength( 100 )]
		[DataMember]
		public string Title { get; set; }
		
		[DataMember]
		public bool System { get; set; }
		
		[DataMember]
		public int? ParentPageId { get; set; }
		
		[DataMember]
		public int? SiteId { get; set; }
		
		[MaxLength( 100 )]
		[DataMember]
		public string Layout { get; set; }
		
		[DataMember]
		public bool RequiresEncryption { get; set; }
		
		[DataMember]
		public bool EnableViewState { get; set; }
		
		[DataMember]
		public bool MenuDisplayDescription { get; set; }
		
		[DataMember]
		public bool MenuDisplayIcon { get; set; }
		
		[DataMember]
		public bool MenuDisplayChildPages { get; set; }
		
		[DataMember]
		public int DisplayInNavWhen { get; set; }
		
		[DataMember]
		public int Order { get; set; }
		
		[DataMember]
		public int OutputCacheDuration { get; set; }
		
		[DataMember]
		public string Description { get; set; }
		
		[DataMember]
		public bool IncludeAdminFooter { get; set; }
		
		[DataMember]
		public DateTime? CreatedDateTime { get; set; }
		
		[DataMember]
		public DateTime? ModifiedDateTime { get; set; }
		
		[DataMember]
		public int? CreatedByPersonId { get; set; }
		
		[DataMember]
		public int? ModifiedByPersonId { get; set; }
		
		[NotMapped]
		public override string AuthEntity { get { return "Cms.Page"; } }

		public virtual ICollection<BlockInstance> BlockInstances { get; set; }

		public virtual ICollection<Page> Pages { get; set; }

		public virtual ICollection<PageRoute> PageRoutes { get; set; }

		public virtual ICollection<Site> Sites { get; set; }

		public virtual Page ParentPage { get; set; }

		public virtual Site Site { get; set; }

		public virtual Crm.Person CreatedByPerson { get; set; }

		public virtual Crm.Person ModifiedByPerson { get; set; }
    }

    public partial class PageConfiguration : EntityTypeConfiguration<Page>
    {
        public PageConfiguration()
        {
			this.HasOptional( p => p.ParentPage ).WithMany( p => p.Pages ).HasForeignKey( p => p.ParentPageId );
			this.HasOptional( p => p.Site ).WithMany( p => p.Pages ).HasForeignKey( p => p.SiteId );
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId );
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId );
		}
    }
}
