DROP VIEW IF EXISTS procurement.supplier_mailboxes_view;

CREATE OR REPLACE VIEW procurement.supplier_mailboxes_view
AS
SELECT
    id,
    supplier_id,
    email
FROM procurement.supplier_mailboxes;