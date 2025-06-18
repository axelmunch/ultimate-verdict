import fillCreateVoteForm from "./fill-create-vote-form";

describe("Create vote", () => {
  it("passes", () => {
    cy.visit("/");

    const voteName = fillCreateVoteForm();

    cy.get("[data-votes]").contains(voteName).click();

    cy.get("[data-round]").eq(0).click();

    cy.get("[data-input]").eq(1).type("{selectall}4");
    cy.get("[data-input]").eq(2).type("{selectall}2");

    cy.get("[data-submit-vote]").click();
    cy.get("[data-submit-vote-confirm]").click();

    cy.get("[data-submit-vote-confirm]").should("not.exist");
  });
});
