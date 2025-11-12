START TRANSACTION;

INSERT INTO platform.feature_flags (code, description) VALUES
                                                           
    ('orders-module', 'Enable the orders module'),
    ('parcel-templates-module', 'Enable the parcel templates module'),
    ('stocks-module', 'Enable the stocks module'),
    ('procurement-module', 'Enable the procurements module'),
    
    ('stocks.statistics-module', 'Enable stocks statistics module'),
    ('stocks.scanning-barcodes-module', 'Enable stocks statistics module'),
    
    ('invoices.auto-generation', 'Enable automatic generation of invoices'),
    ('invoices.allegro-sync', 'Enable invoice synchronization with Allegro'),
    ('invoices.shopify-sync', 'Enable invoice synchronization with Shopify');
    
COMMIT;