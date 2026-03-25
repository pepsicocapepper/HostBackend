-- Created by eric on 3/5/26 - 09:39:22


CREATE TABLE menu
(
    id            SERIAL PRIMARY KEY,
    name          TEXT NOT NULL UNIQUE,
    pos_name      TEXT,
    color         BYTEA,
    menu_id       INT,
    display_order INT  NOT NULL,
    FOREIGN KEY (menu_id) REFERENCES menu (id)
);

CREATE OR REPLACE FUNCTION assign_order_menu()
    RETURNS TRIGGER AS
$$
BEGIN
    SELECT COALESCE(MAX(display_order), 0) + 1
    INTO NEW.display_order
    FROM menu
    WHERE menu_id IS NOT DISTINCT FROM NEW.menu_id;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_display_order_menu
    BEFORE INSERT
    ON menu
    FOR EACH ROW
EXECUTE FUNCTION assign_order_menu();