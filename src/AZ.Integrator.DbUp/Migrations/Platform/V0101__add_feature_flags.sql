START TRANSACTION;

INSERT INTO platform.feature_flags (code, description) VALUES


    ('marketplace-module', 'Enable the marketplace module'),
    ('marketplace.orders-module', 'Enable the orders module'),
    ('marketplace.parcel-templates-module', 'Enable the parcel templates module'),

    ('warehouse-module', 'Enable the warehouse module'),
    ('warehouse.stocks-module', 'Enable the warehouse stocks module'),
    ('warehouse.statistics-module', 'Enable the warehouse statistics module'),
    ('warehouse.scanning-barcodes-module', 'Enable the warehouse barcodes scanning module'),
    
    ('procurement-module', 'Enable the procurements module'),
    
    ('invoices.auto-generation', 'Enable automatic generation of invoices'),
    ('invoices.allegro-sync', 'Enable invoice synchronization with Allegro'),
    ('invoices.shopify-sync', 'Enable invoice synchronization with Shopify');
    
COMMIT;