DROP VIEW IF EXISTS integration.inpost_view;

CREATE OR REPLACE VIEW integration.inpost_view
AS
SELECT
    tenant_id,
    organization_id,
    access_token,
    display_name,
    is_enabled,
    created_at,
    updated_at,
    sender_name,
    sender_company_name,
    sender_first_name,
    sender_last_name,
    sender_email,
    sender_phone,
    sender_address_street,
    sender_address_building_number,
    sender_address_city,
    sender_address_post_code,
    sender_address_country_code
FROM integration.inpost;