#![feature(plugin)]
#![plugin(rocket_codegen)]

extern crate rocket;

#[get("/")]
fn index() -> &'static str {
        "hale.rs v0.1.0"
}

fn main() {
        rocket::ignite().mount("/", routes![index]).launch();
}
