START TRANSACTION;

ALTER TABLE tag_parcel_templates ADD COLUMN tenant_id uuid NOT NULL;
ALTER TABLE tag_parcels ADD COLUMN tenant_id uuid NOT NULL;

-- Change primary key to include tenant_id
ALTER TABLE tag_parcels DROP CONSTRAINT "FK_tag_parcels_tag_parcel_templates_tag";
ALTER TABLE tag_parcel_templates DROP CONSTRAINT "PK_tag_parcel_templates";

ALTER TABLE tag_parcel_templates ADD CONSTRAINT "PK_tag_parcel_templates_new" PRIMARY KEY (tag, tenant_id);
ALTER TABLE tag_parcels ADD CONSTRAINT "FK_tag_parcels_tag_tenant_id_parcel_templates_tag_tenant_id" FOREIGN KEY (tag, tenant_id) REFERENCES tag_parcel_templates (tag, tenant_id) ON DELETE CASCADE;

COMMIT;