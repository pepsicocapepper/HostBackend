-- Created by eric on 3/5/26 - 09:58:11

CREATE FUNCTION check_menu_has_parent()
    RETURNS TRIGGER AS
$$
BEGIN

    IF EXISTS (
        SELECT 1 FROM menu WHERE id = NEW.menu_id AND menu_id IS NULL
    ) THEN
        RAISE EXCEPTION 'Cannot add items to root menu';
    END IF;

    IF EXISTS (
        SELECT 1
        FROM menu m
                 JOIN menu parent ON parent.id = m.menu_id
        WHERE m.id = NEW.menu_id
          AND parent.menu_id IS NOT NULL
    ) THEN
        RAISE EXCEPTION 'Cannot add items deeper than level 1';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TABLE menu_item
(
    menu_id       INT NOT NULL,
    item_id       INT NOT NULL,
    display_order INT NOT NULL,
    PRIMARY KEY (menu_id, item_id),
    FOREIGN KEY (menu_id) REFERENCES menu (id),
    FOREIGN KEY (item_id) REFERENCES item (id)
);

CREATE TRIGGER trg_validate_menu
    BEFORE INSERT OR UPDATE
    ON menu_item
    FOR EACH ROW
EXECUTE FUNCTION check_menu_has_parent();