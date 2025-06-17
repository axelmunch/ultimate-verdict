import Weighted from "../../src/components/voting_system.tsx/Weighted";
import type { Option } from "../../src/types";

describe("Weighted.cy.tsx", () => {
  it("mounts", () => {
    cy.mount(
      <Weighted options={[]} setCanSubmit={() => {}} setDecisions={() => {}} />,
    );

    cy.get("[data-component]").should("exist");
  });

  it("has right options in the default order", () => {
    cy.fixture("options.json").then((optionsData: Option[]) => {
      const options = optionsData.map((option) => ({
        ...option,
      }));

      cy.mount(
        <Weighted
          options={options}
          setCanSubmit={() => {}}
          setDecisions={() => {}}
        />,
      );

      cy.get("[data-option]")
        .should("have.length", optionsData.length)
        .each((el, index) => {
          expect(el.text()).to.contains(optionsData[index].name);
        });
    });
  });
});
